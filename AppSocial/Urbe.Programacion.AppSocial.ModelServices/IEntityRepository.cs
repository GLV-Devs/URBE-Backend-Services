
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IEntityRepository<TEntity, TKey>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<TEntity?> Find(TKey key);
    public IQueryable<TEntity> Query();
    public ValueTask<TEntity?> Find(SocialAppUser? Requester, TKey key);
    public IQueryable<TEntity> Query(SocialAppUser? Requester);
    public ValueTask<int> SaveChanges();
}
