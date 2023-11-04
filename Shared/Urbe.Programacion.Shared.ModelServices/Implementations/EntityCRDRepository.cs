using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.ModelServices.Implementations;

public abstract class EntityCRDRepository<TEntity, TKey, TCreationModel> : EntityRepository<TEntity, TKey>, IEntityCRDRepository<TEntity, TKey, TCreationModel>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected EntityCRDRepository(DbContext context, IServiceProvider provider) : base(context, provider)
    {
    }

    public abstract ValueTask<SuccessResult<TEntity>> Create(BaseAppUser? requester, TCreationModel model);

    public abstract ValueTask<SuccessResult<object>> GetView(BaseAppUser? requester, TEntity entity);

    public virtual ValueTask<SuccessResult> Delete(BaseAppUser? requester, TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return ValueTask.FromResult(SuccessResult.Success);
    }

    public abstract ValueTask<IQueryable<object>?> GetViews(BaseAppUser? requester, IQueryable<TEntity>? users);
}
