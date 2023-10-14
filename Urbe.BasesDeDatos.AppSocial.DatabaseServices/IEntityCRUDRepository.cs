using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IEntityCRUDRepository<TEntity, TCreationModel, TUpdateModel> : IEntityCRDRepository<TEntity, TCreationModel>
    where TEntity : IEntity
{
    public ValueTask<ErrorList> Update(TEntity entity, TUpdateModel update);
}
