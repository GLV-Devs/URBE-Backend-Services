using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IEntityCRDRepository<TEntity, TCreationModel> where TEntity : IEntity
{
    public ValueTask<SuccessResult<TEntity>> Create(TCreationModel model);
    
    public ValueTask<object> GetView(TEntity entity);

    public ValueTask<bool> Delete(TEntity entity, SocialContext context)
    {
        context.Remove(entity);
        return ValueTask.FromResult(true);
    }
}
