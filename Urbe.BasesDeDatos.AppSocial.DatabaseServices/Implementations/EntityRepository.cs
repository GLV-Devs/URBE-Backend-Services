using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;

public class EntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected readonly SocialContext context;

    public EntityRepository(SocialContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual async ValueTask<TEntity?> Find(SocialAppUser Requester, TKey key)
        => await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(key));

    public virtual IQueryable<TEntity> Query(SocialAppUser Requester)
        => context.Set<TEntity>();

    public virtual async ValueTask<int> SaveChanges()
        => await context.SaveChangesAsync();
}
