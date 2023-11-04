using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Interfaces;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.Common;
using Urbe.Programacion.AppSocial.ModelServices;

namespace Urbe.Programacion.AppSocial.ModelServices.Implementations;

public abstract class EntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel>
    : EntityCRDRepository<TEntity, TKey, TCreationModel>, IEntityCRUDRepository<TEntity, TKey, TCreationModel, TUpdateModel>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected EntityCRUDRepository(SocialContext context, IServiceProvider provider) : base(context, provider)
    {
    }

    public abstract ValueTask<SuccessResult> Update(SocialAppUser? requester, TEntity entity, TUpdateModel update);
}