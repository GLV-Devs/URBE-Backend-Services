namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface ICRUDEntity<TEntity, TUpdateModel, TCreationModel>
    : IUpdateable<TUpdateModel>, ICreateable<TEntity, TCreationModel>, IReadable, IDeletable
    where TEntity : IEntity
{

}
