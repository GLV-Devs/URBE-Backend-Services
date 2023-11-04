using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.API.Services;

public class SocialAppUserStore : UserOnlyStore<SocialAppUser, SocialContext, Guid>
{
    private static readonly Task<SocialAppUser?> CompletedEmptyTask = Task.FromResult<SocialAppUser?>(null);

    public SocialAppUserStore(SocialContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    {
    }

    public override Task<SocialAppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        return Guid.TryParse(userId, out Guid id)
            ? Context.SocialAppUsers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            : CompletedEmptyTask;
    }

    public override Guid ConvertIdFromString(string? id) 
        => id is not null ? Guid.Parse(id, null) : default;

    public override string? ConvertIdToString(Guid id)
        => id.ToString();
}
