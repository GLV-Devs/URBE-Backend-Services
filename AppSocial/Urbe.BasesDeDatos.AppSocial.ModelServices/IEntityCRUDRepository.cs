using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.Common;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> : IEntityCRDRepository<TEntity, TKey, TCreationModel>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<SuccessResult> Update(SocialAppUser? requester, TEntity entity, TUpdateModel update);
}
