using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.ModelServices;

public interface IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel> : IEntityCRDRepository<TEntity, TKey, TCreationModel>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<SuccessResult> Update(BaseAppUser? requester, TEntity entity, TUpdateModel update);
}
