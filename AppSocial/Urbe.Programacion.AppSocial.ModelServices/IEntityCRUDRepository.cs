using Urbe.Programacion.AppSocial.Common;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> : IEntityCRDRepository<TEntity, TKey, TCreationModel>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<SuccessResult> Update(SocialAppUser? requester, TEntity entity, TUpdateModel update);
}
