using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IEntityCRDRepository<TEntity, TKey, TCreationModel> : IEntityRepository<TEntity, TKey>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<SuccessResult<TEntity>> Create(SocialAppUser? requester, TCreationModel model);
    
    public ValueTask<object> GetView(SocialAppUser requester, TEntity entity);

    public ValueTask<bool> Delete(SocialAppUser requester, TEntity entity);
}
