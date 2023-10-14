using System.Data.Common;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;

public abstract class EntityCRDRepository<TEntity, TKey, TCreationModel> : EntityRepository<TEntity, TKey>, IEntityCRDRepository<TEntity, TKey, TCreationModel>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected EntityCRDRepository(SocialContext context, IServiceProvider provider) : base(context, provider)
    {
    }

    public abstract ValueTask<SuccessResult<TEntity>> Create(SocialAppUser? requester, TCreationModel model);

    public abstract ValueTask<SuccessResult<object>> GetView(SocialAppUser requester, TEntity entity);

    public virtual ValueTask<bool> Delete(SocialAppUser requester, TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return ValueTask.FromResult(true);
    }
}
