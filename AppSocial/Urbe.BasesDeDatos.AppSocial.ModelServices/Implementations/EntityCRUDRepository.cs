using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.Implementations;

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