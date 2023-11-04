using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.Shared.ModelServices.Implementations;

public abstract class EntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel>
    : EntityCRDRepository<TEntity, TKey, TCreationModel>, IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected EntityCRUDRepository(DbContext context, IServiceProvider provider) : base(context, provider)
    {
    }

    public abstract ValueTask<SuccessResult> Update(BaseAppUser? requester, TEntity entity, TUpdateModel update);
}