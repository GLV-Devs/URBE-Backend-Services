using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IEntityRepository<TEntity, TKey> 
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<TEntity?> Find(TKey key);
    public IQueryable<TEntity> Query();
    public ValueTask<TEntity?> Find(SocialAppUser Requester, TKey key);
    public IQueryable<TEntity> Query(SocialAppUser Requester);
    public ValueTask<int> SaveChanges();
}
