namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface ICRUDEntity<TEntity, TUpdateModel, TCreationModel>
    : IUpdateable<TEntity, TUpdateModel>, ICreateable<TEntity, TCreationModel>, IReadable<TEntity>, IDeletable<TEntity>
    where TEntity : IEntity
{

}
