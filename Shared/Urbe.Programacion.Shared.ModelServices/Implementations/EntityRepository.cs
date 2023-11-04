using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.ModelServices.Implementations;

public class EntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected readonly DbContext context;
    protected readonly IServiceProvider provider;

    public EntityRepository(DbContext context, IServiceProvider provider)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public virtual async ValueTask<TEntity?> Find(TKey key)
        => await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(key));

    public virtual IQueryable<TEntity> Query()
        => context.Set<TEntity>();

    public virtual async ValueTask<TEntity?> Find(BaseAppUser? Requester, TKey key)
        => await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(key));

    public virtual IQueryable<TEntity> Query(BaseAppUser? Requester)
        => context.Set<TEntity>();

    public virtual async ValueTask<int> SaveChanges()
        => await context.SaveChangesAsync();
}
