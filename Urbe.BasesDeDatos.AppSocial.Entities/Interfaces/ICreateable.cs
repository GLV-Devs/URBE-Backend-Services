using Urbe.BasesDeDatos.AppSocial.Common;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface ICreateable<TEntity, TCreationModel> where TEntity : IEntity
{
    public static abstract ValueTask<SuccessResult<TEntity>> Create(TCreationModel model);
}
