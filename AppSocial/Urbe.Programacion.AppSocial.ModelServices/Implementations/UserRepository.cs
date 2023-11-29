using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Models;
using Urbe.Programacion.Shared.ModelServices;
using Urbe.Programacion.Shared.ModelServices.Implementations;
using Urbe.Programacion.Shared.Services.Attributes;
using Urbe.Programacion.AppSocial.DataTransfer;
using System.Net;
using Microsoft.EntityFrameworkCore.Internal;

namespace Urbe.Programacion.AppSocial.ModelServices.Implementations;

[RegisterService(typeof(IEntityCRDRepository<SocialAppUser, Guid, UserCreationModel>))]
[RegisterService(typeof(IEntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>))]
[RegisterService(typeof(IUserRepository))]
public class UserRepository : EntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>, IUserRepository
{
    protected readonly UserManager<SocialAppUser> userManager;
    protected new readonly SocialContext context;

    public UserRepository(SocialContext context, UserManager<SocialAppUser> userManager, IServiceProvider provider) : base(context, provider)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.context = context;
    }

    public override async ValueTask<SuccessResult> Delete(BaseAppUser? requester, SocialAppUser entity)
    {
        var result = await userManager.DeleteAsync(entity);
        if (result.Succeeded is false)
        {
            ErrorList errors = new();
            foreach (var error in result.Errors)
                errors.AddError(ErrorMessages.TryBindError(error.Code, error.Description));
            return new SuccessResult(errors);
        }

        return SuccessResult.Success;
    }

    public override async ValueTask<SuccessResult> Update(BaseAppUser? r, SocialAppUser entity, UserUpdateModel update)
    {
        var requester = (SocialAppUser?)r;

        var errors = new ErrorList();

        if (requester is null || requester.Id != entity.Id)
        {
            errors.AddError(ErrorMessages.NoPermission());
            errors.RecommendedCode = System.Net.HttpStatusCode.Unauthorized;
        }

        if (Helper.IsUpdatingString(entity.Email, update.Email)
                && Helper.IsTooLong(ref errors, update.Email, BaseAppUser.EmailMaxLength, "Correo electronico") is false)
        {
            errors.AddError(ErrorMessages.NotSupported("Correo Electrónico", "Cambiar"));
            //var match = Regexes.Email().Match(update.Email);
            //if (match.Success is false || match.Length != update.Email.Length)
            //    errors.AddError(ErrorMessages.BadEmail(update.Email));
            //else
            //{
            //    userManager.Email
            //    entity.EmailConfirmed = false;
            //    entity.Email = update.Email;
            //}
        }

        if (Helper.IsUpdatingString(entity.UserName, update.Username)
                && Helper.IsTooLong(ref errors, update.Username, BaseAppUser.UserNameMaxLength, "Nombre de Usuario") is false)
        {
            if (await userManager.FindByNameAsync(update.Username) is not null)
                errors.AddError(ErrorMessages.UsernameAlreadyInUse(update.Username));
            else
            {
                var result = await userManager.SetUserNameAsync(entity, update.Username);
                if (result.Succeeded is false)
                    errors.AddError(ErrorMessages.BadUsername(update.Username));
            }
        }

        if (Helper.IsUpdatingString(entity.RealName, update.RealName, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.RealName, BaseAppUser.RealNameMaxLength, "Nombre Real") is false)
            entity.RealName = update.RealName;

        if (Helper.IsUpdatingString(entity.Pronouns, update.Pronouns, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.Pronouns, SocialAppUser.PronounsMaxLength, "Pronombres") is false)
            entity.Pronouns = update.Pronouns;

        if (Helper.IsUpdatingString(entity.ProfileMessage, update.ProfileMessage, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.ProfileMessage, BaseAppUser.ProfileMessageMaxLength, "Mensaje de Perfil") is false)
            entity.ProfileMessage = update.ProfileMessage;

        if (Helper.IsUpdatingString(entity.ProfilePictureUrl, update.ProfilePictureUrl, StringComparison.Ordinal)
                && Helper.IsTooLong(ref errors, update.ProfilePictureUrl, BaseAppUser.ProfilePictureUrlMaxLength, "URL de Foto de Perfil") is false)
            entity.ProfilePictureUrl = update.ProfilePictureUrl;

        if (Helper.IsUpdating(entity.Settings, update.UserSettings))
            entity.Settings = update.UserSettings.Value;

        return new(errors);
    }

    public override async ValueTask<SuccessResult<SocialAppUser>> Create(BaseAppUser? requester, UserCreationModel model)
    {
        var errors = new ErrorList();

        if (Helper.IsTooLong(ref errors, model.Username, BaseAppUser.UserNameMaxLength, "Nombre de Usuario") is false
            && await userManager.FindByNameAsync(model.Username) is not null)
        {
            errors.AddError(ErrorMessages.UsernameAlreadyInUse(model.Username));
            errors.RecommendedCode = HttpStatusCode.Conflict;
            return new SuccessResult<SocialAppUser>(errors);
        }

        if (Helper.IsTooLong(ref errors, model.Email, BaseAppUser.EmailMaxLength, "Correo Electrónico") is false
            && await userManager.FindByEmailAsync(model.Email) is not null)
        {
            errors.AddError(ErrorMessages.EmailAlreadyInUse(model.Email));
            errors.RecommendedCode = HttpStatusCode.Conflict;
            return new SuccessResult<SocialAppUser>(errors);
        }

        var newuser = new SocialAppUser()
        {
            Id = Guid.NewGuid(),
            UserName = model.Username,
            Email = model.Email,
            RealName = model.RealName,
            Pronouns = model.Pronouns
        };

        var createresult = await userManager.CreateAsync(newuser);
        if (createresult.Succeeded is false)
        {
            foreach (var error in createresult.Errors)
                errors.AddError(new ErrorMessage(
                    error.Description,
                    error.Code,
                    null
                ));

            errors.RecommendedCode = HttpStatusCode.BadRequest;

            return new SuccessResult<SocialAppUser>(errors);
        }

        var passresults = await userManager.AddPasswordAsync(newuser, model.Password);
        if (passresults.Succeeded is false)
        {
            await userManager.DeleteAsync(newuser);
            errors.AddError(ErrorMessages.BadPassword());
            foreach (var error in passresults.Errors)
                errors.AddError(new ErrorMessage(
                    error.Description,
                    error.Code,
                    null
                ));

            errors.RecommendedCode = HttpStatusCode.BadRequest;
            return new SuccessResult<SocialAppUser>(errors);
        }
        //else
        //{
        //    var token = await userManager.GenerateChangeEmailTokenAsync(newuser, model.Email);
        //    if ((await userManager.ChangeEmailAsync(newuser, model.Email, token)).Succeeded is false)
        //    {
        //        await userManager.DeleteAsync(newuser);
        //        errors.AddError(ErrorMessages.BadPassword());
        //        return new SuccessResult<SocialAppUser>(errors);
        //    }
        //}

        await context.SaveChangesAsync();
        return new SuccessResult<SocialAppUser>(newuser);
    }

    public ValueTask<SuccessResult<object>> GetSelfView(SocialAppUser user)
        => ValueTask.FromResult(new SuccessResult<object>(new UserSelfViewModel()
        {
            Email = user.Email,
            EmailVerified = user.EmailConfirmed,
            UserId = user.Id,
            ProfileMessage = user.ProfileMessage,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Pronouns = user.Pronouns,
            RealName = user.RealName,
            Settings = user.Settings,
            Username = user.UserName!
        }));

    public override async ValueTask<SuccessResult<object>> GetView(BaseAppUser? r, SocialAppUser entity)
    {
        var requester = (SocialAppUser?)r;

        if (requester is null || requester.Id != entity.Id)
        {
            UserViewModel view;
            if (await CanView(requester, entity))
            {
                view = new UserViewModel()
                {
                    UserId = entity.Id,
                    Username = entity.UserName!,
                    ProfilePictureUrl = entity.ProfilePictureUrl,
                    Pronouns = entity.Pronouns
                };

                if (requester is not null)
                {
                    view.FollowsRequester = await IsFollowing(entity, requester);
                    view.IsFollowedByRequester = await IsFollowing(requester, entity); 
                }
            }
            else
                view = new UserViewModel()
                {
                    UserId = entity.Id,
                    Username = entity.UserName!,
                    ProfilePictureUrl = entity.ProfilePictureUrl,
                    RealName = entity.Settings.HasFlag(UserSettings.AllowRealNamePublicly) ? entity.RealName : null,
                    Pronouns = entity.Pronouns
                };

            return new SuccessResult<object>(view);
        }

        return new SuccessResult<object>(new UserViewModel()
        {
            UserId = entity.Id,
            Username = entity.UserName!,
            ProfilePictureUrl = entity.ProfilePictureUrl,
            RealName = entity.RealName,
            FollowsRequester = false,
            IsFollowedByRequester = await IsFollowing(requester, entity),
            ProfileMessage = entity.ProfileMessage,
            Pronouns = entity.Pronouns
        });
    }

    public async ValueTask<SocialAppUser?> FindByUsername(string username)
        => await userManager.FindByNameAsync(username);

    public async ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser followed) 
        => requester.Id != followed.Id
            && (followed.Followers?.Contains(requester) is true
            || await context.SocialAppUserFollows.AnyAsync(x => x.FollowerId == requester.Id && x.FollowedId == followed.Id));

    public ValueTask<IQueryable<SocialAppUser>> GetFollowers(SocialAppUser requester)
        => ValueTask.FromResult(context.SocialAppUserFollows.Where(x => x.FollowedId == requester.Id).Select(x => x.Follower!));

    public ValueTask<IQueryable<SocialAppUser>> GetFollowing(SocialAppUser requester)
        => ValueTask.FromResult(context.SocialAppUserFollows.Where(x => x.FollowerId == requester.Id).Select(x => x.Followed!));

    public ValueTask<IQueryable<SocialAppUser>> GetMutuals(SocialAppUser requester)
        => ValueTask.FromResult(
            context.SocialAppUsers.Where(x => 
                requester.Followers != null 
                && requester.Followers.Contains(x)
                && x.Followers != null 
                && x.Followers.Contains(requester)
            )
        );

    public async ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser follower, SocialAppUser followed)
        => await CanView(requester, follower) && await IsFollowing(follower, followed);

    public async ValueTask<IQueryable<SocialAppUser>?> GetFollowers(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetFollowers(user) : null;

    public async ValueTask<IQueryable<SocialAppUser>?> GetFollowing(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetFollowing(user) : null;

    public async ValueTask<IQueryable<SocialAppUser>?> GetMutuals(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetMutuals(user) : null;

    public async ValueTask<bool> FollowUser(SocialAppUser requester, SocialAppUser followed)
    {
        if (requester.Id == followed.Id || await IsFollowing(requester, followed))
            return false;

        context.SocialAppUserFollows.Add(new SocialAppUserFollow()
        {
            Followed = followed,
            Follower = requester
        });

        return true;
    }

    public async ValueTask<bool> UnfollowUser(SocialAppUser requester, SocialAppUser followed)
    {
        return requester.Id != followed.Id
            && await context.SocialAppUserFollows.Where(x => x.Follower == requester && x.Followed == followed).ExecuteDeleteAsync() > 0;
    }

    private readonly record struct FollowerRelation(Guid FollowedId, Guid FollowerId);
    public override ValueTask<IQueryable<object>?> GetViews(BaseAppUser? r, IQueryable<SocialAppUser>? users)
    {
        var requester = (SocialAppUser?)r;
        if (users is null)
            return ValueTask.FromResult<IQueryable<object>?>(null);
        else
        {
            return ValueTask.FromResult<IQueryable<object>?>(
                (IQueryable<UserViewModel>?)(requester is null
                ? users.AsNoTracking().Select(x => x.Settings.HasFlag(UserSettings.AllowAnonymousViews)
                    ? new UserViewModel() // Anonymous requester
                    {
                        UserId = x.Id,
                        ProfileMessage = x.ProfileMessage,
                        ProfilePictureUrl = x.ProfilePictureUrl,
                        Pronouns = x.Pronouns,
                        RealName = x.RealName,
                        Username = x.UserName!
                    }
                    : new UserViewModel()
                    {
                        UserId = x.Id,
                        Username = x.UserName!,
                        ProfilePictureUrl = null,
                        Pronouns = x.Pronouns,
                    })
                : users.AsNoTracking()
                    //.Join(
                    //    context.SocialAppUserFollows,
                    //    o => new FollowerRelation(requester.Id, o.Id),
                    //    i => new FollowerRelation(i.FollowerId, i.FollowedId),
                    //)
                    .Select(x => x.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || (x.Followers != null && x.Followers.Contains(requester))
                    ? new UserViewModel()
                    {
                        UserId = x.Id,
                        ProfileMessage = x.ProfileMessage,
                        ProfilePictureUrl = x.ProfilePictureUrl,
                        Pronouns = x.Pronouns,
                        RealName = x.RealName,
                        Username = x.UserName!,
                        //FollowsRequester = x.FollowedUsers != null && x.FollowedUsers.Contains(requester),
                        IsFollowedByRequester = x.Followers != null && x.Followers.Contains(requester)
                    }
                    : new UserViewModel()
                    {
                        UserId = x.Id,
                        Username = x.UserName!,
                        Pronouns = x.Pronouns,
                        ProfilePictureUrl = x.ProfilePictureUrl,
                        //FollowsRequester = x.FollowedUsers != null && x.FollowedUsers.Contains(requester),
                        IsFollowedByRequester = x.Followers != null && x.Followers.Contains(requester)
                    }))
               );
        }
    }

    public IQueryable<SocialAppUser> Query(SocialAppUser? r, string userName)
        => Query(r).Where(x => EF.Functions.Like(x.UserName!, userName));

    public override IQueryable<SocialAppUser> Query(BaseAppUser? r)
    {
        var requester = (SocialAppUser?)r;
        return requester is not null
                ? Query().Where(x => x.Id != requester.Id && (x.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || x.Followers != null && x.Followers.Contains(requester)))
                : Query().Where(x => x.Settings.HasFlag(UserSettings.AllowAnonymousViews));
    }

    private async ValueTask<bool> CanView(SocialAppUser? requester, SocialAppUser entity)
        => requester?.Id.Equals(entity.Id) is true
        || (entity.Settings.HasFlag(UserSettings.AllowAnonymousViews) || requester is not null)
            && (entity.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || requester is not null && await IsFollowing(requester, entity));
}
