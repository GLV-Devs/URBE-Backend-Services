using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.ModelServices;

public interface IEntityRepository<TEntity, TKey>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<TEntity?> Find(TKey key);
    public IQueryable<TEntity> Query();
    public ValueTask<TEntity?> Find(BaseAppUser? Requester, TKey key);
    public IQueryable<TEntity> Query(BaseAppUser? Requester);
    public ValueTask<int> SaveChanges();
}
