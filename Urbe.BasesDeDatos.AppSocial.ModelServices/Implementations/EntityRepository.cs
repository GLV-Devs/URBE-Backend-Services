using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.Implementations;

public class EntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected readonly SocialContext context;
    protected readonly IServiceProvider provider;

    public EntityRepository(SocialContext context, IServiceProvider provider)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public virtual async ValueTask<TEntity?> Find(TKey key)
        => await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(key));

    public virtual IQueryable<TEntity> Query()
        => context.Set<TEntity>();

    public virtual async ValueTask<TEntity?> Find(SocialAppUser? Requester, TKey key)
        => await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(key));

    public virtual IQueryable<TEntity> Query(SocialAppUser? Requester)
        => context.Set<TEntity>();

    public virtual async ValueTask<int> SaveChanges()
        => await context.SaveChangesAsync();
}
