using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mail.NET;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.Shared.Services.Attributes;
using Urbe.Programacion.AppSocial.Common;

namespace Urbe.Programacion.AppSocial.ModelServices.Implementations;

public class UserVerificationServiceOptions
{
    public string? MailBody { get; }
    public string? TokenFormat { get; }
    public string? MailFromAddress { get; }
    public MailBodyType MailBodyType { get; }

    public UserVerificationServiceOptions(string? mailBody, string? tokenFormat, MailBodyType mailBodyType, string mailFromAddress)
    {
        if (mailBody is not null && mailBody.Contains("{token}", StringComparison.OrdinalIgnoreCase) is false)
            throw new ArgumentException("If mailBody is not null, then it must contain the {token} placeholder", nameof(mailBody));

        if (tokenFormat is not null && tokenFormat.Contains("{token}", StringComparison.OrdinalIgnoreCase) is false)
            throw new ArgumentException("If tokenFormat is not null, then it must contain the {token} placeholder", nameof(tokenFormat));

        MailBodyType = mailBodyType;

        MailFromAddress = mailFromAddress;
    }
}

[RegisterService(typeof(IUserVerificationService))]
public class UserVerificationService : IUserVerificationService
{
    private readonly SocialContext context;
    private readonly IUserRepository UserRepository;
    private readonly UserManager<SocialAppUser> UserManager;
    private readonly IMailWriter MailWriter;
    private readonly ILogger<UserVerificationService> Logger;
    private readonly UserVerificationServiceOptions Options;

    public UserVerificationService(IOptions<UserVerificationServiceOptions> options, SocialContext context, IUserRepository userRepository, UserManager<SocialAppUser> userManager, IMailWriter mailWriter, ILogger<UserVerificationService> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        MailWriter = mailWriter ?? throw new ArgumentNullException(nameof(mailWriter));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Options = options.Value;
    }

    public async ValueTask<PendingMailConfirmation?> FindById(RandomKey key)
        => await context.PendingMailConfirmations.AsNoTracking().Include(x => x.User).Where(x => x.Id == key).FirstOrDefaultAsync();

    public async ValueTask<PendingMailConfirmation?> FindByUser(SocialAppUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return await context.PendingMailConfirmations.AsNoTracking().Include(x => x.User).Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
    }

    public async ValueTask<PendingMailConfirmation?> FindByToken(string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(token);
        return await context.PendingMailConfirmations.AsNoTracking().Include(x => x.User).Where(x => x.Token == token).FirstOrDefaultAsync();
    }

    public async ValueTask<VerificationResult> VerifyUserEmail(PendingMailConfirmation confirmation)
    {
        if (DateTimeOffset.Now > confirmation.CreationDate + confirmation.Validity)
            return VerificationResult.Expired;

        context.PendingMailConfirmations.Remove(confirmation);
        await context.SaveChangesAsync();

        if (confirmation.User is not SocialAppUser user)
        {
            user = (await UserRepository.Find(confirmation.UserId))!;
            if (user is null)
                return VerificationResult.Rejected;
        }

        var result = await UserManager.ConfirmEmailAsync(user, confirmation.Token);

        if (result.Succeeded is false)
        {
            StringBuilder sb = new();
            foreach (var error in result.Errors)
                sb.Append("\t ->").Append(error.Code).Append(": ").Append(error.Description).AppendLine();
            Logger.LogError("Errors ocurred while confirming the email: {errorlist}", sb.ToString());

            return VerificationResult.Rejected;
        }

        return VerificationResult.Approved;
    }

    public async ValueTask<bool> CreateMailConfirmation(SocialAppUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (user.Email is null)
            throw new ArgumentException("User must have a registered email", nameof(user));

        if (await FindByUser(user) is not null || await UserManager.IsEmailConfirmedAsync(user))
            return false;

        var pmc = new PendingMailConfirmation()
        {
            Id = RandomKey.NewHashKey(),
            CreationDate = DateTimeOffset.Now,
            Token = await UserManager.GenerateEmailConfirmationTokenAsync(user),
            User = user,
            Validity = TimeSpan.FromHours(6)
        };

        context.Add(pmc);
        await context.SaveChangesAsync();

        var mailBody = Options.MailBody;
        var tokenFormat = Options.TokenFormat;
        var bodyType = Options.MailBodyType;

        var body = mailBody?.Replace("{token}", tokenFormat?.Replace("{token}", pmc.Token) ?? pmc.Token) ?? pmc.Token;
        BackgroundTaskStore.Add(async ct =>
        {
            var msg = new MailDraft()
            {
                Body = body,
                BodyType = bodyType,
                From = Options.MailFromAddress,
                Importance = MessageImportance.Normal
            };
            msg.Recipients.Add(user.Email);

            Logger.LogInformation("Attempting to send Email verification mail message for user {username} ({userid})", user.UserName, user.Id);
            try
            {
                await MailWriter.SendMessage(msg, ct);
                Logger.LogInformation("Succesfully sent Email verification mail message for user {username} ({userid})", user.UserName, user.Id);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "An error ocurred while trying to send Email verification mail message for user {username} ({userid})", user.UserName, user.Id);
            }
        });

        return true;
    }
}
