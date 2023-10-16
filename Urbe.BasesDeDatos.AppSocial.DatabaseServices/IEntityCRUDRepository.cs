using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> : IEntityCRDRepository<TEntity, TKey, TCreationModel>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<ErrorList> Update(SocialAppUser? requester, TEntity entity, TUpdateModel update);
}
