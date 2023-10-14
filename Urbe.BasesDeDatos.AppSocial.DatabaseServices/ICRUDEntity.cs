namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface ICRUDEntity<TEntity, TUpdateModel, TCreationModel>
    : IUpdateable<TUpdateModel>, ICreateable<TEntity, TCreationModel>, IReadable, IDeletable
    where TEntity : IEntity
{

}
