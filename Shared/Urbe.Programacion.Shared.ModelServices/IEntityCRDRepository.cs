using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.ModelServices;

public interface IEntityCRDRepository<TEntity, TKey, TCreationModel> : IEntityRepository<TEntity, TKey>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<SuccessResult<TEntity>> Create(BaseAppUser? requester, TCreationModel model);

    public ValueTask<SuccessResult<object>> GetView(BaseAppUser? requester, TEntity entity);

    public ValueTask<IQueryable<object>?> GetViews(BaseAppUser? requester, IQueryable<TEntity>? users);

    public ValueTask<SuccessResult> Delete(BaseAppUser? requester, TEntity entity);
}
