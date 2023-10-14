namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IDeletable<TEntity> where TEntity : IEntity
{
    public static abstract ValueTask<bool> Delete(TEntity entity);
}
