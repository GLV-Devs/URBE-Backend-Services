using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.API.Common.Services;

public class AppRoleStore<TAppRole, TContext> : RoleStore<TAppRole, TContext, Guid>
    where TAppRole : BaseAppRole
    where TContext : DbContext
{
    private static readonly Task<TAppRole?> CompletedEmptyTask = Task.FromResult<TAppRole?>(null);

    public AppRoleStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    { }

    public override Guid ConvertIdFromString(string? id)
        => id is not null ? Guid.Parse(id, null) : default;

    public override string? ConvertIdToString(Guid id)
        => id.ToString();

    public override Task<TAppRole?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        return Guid.TryParse(userId, out Guid id)
            ? Context.Set<TAppRole>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            : CompletedEmptyTask;
    }

}

public class AppUserStore<TAppUser, TAppRole, TContext> : UserStore<TAppUser, TAppRole, TContext, Guid>
    where TAppUser : BaseAppUser
    where TAppRole : BaseAppRole
    where TContext : DbContext
{
    private static readonly Task<TAppUser?> CompletedEmptyTask = Task.FromResult<TAppUser?>(null);

    public AppUserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    { }

    public override Task<TAppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        return Guid.TryParse(userId, out Guid id)
            ? Context.Set<TAppUser>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            : CompletedEmptyTask;
    }

    public override Guid ConvertIdFromString(string? id)
        => id is not null ? Guid.Parse(id, null) : default;

    public override string? ConvertIdToString(Guid id)
        => id.ToString();
}

public class AppUserStore<TAppUser, TContext> : UserOnlyStore<TAppUser, DbContext, Guid>
    where TAppUser : BaseAppUser
    where TContext : DbContext
{
    private static readonly Task<TAppUser?> CompletedEmptyTask = Task.FromResult<TAppUser?>(null);

    public AppUserStore(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    {
    }

    public override Task<TAppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        return Guid.TryParse(userId, out Guid id)
            ? Context.Set<TAppUser>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            : CompletedEmptyTask;
    }

    public override Guid ConvertIdFromString(string? id)
        => id is not null ? Guid.Parse(id, null) : default;

    public override string? ConvertIdToString(Guid id)
        => id.ToString();
}
