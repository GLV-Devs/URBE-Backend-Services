﻿using Urbe.Programacion.AppSocial.Common;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.AppSocial.ModelServices.Implementations;

public abstract class EntityCRDRepository<TEntity, TKey, TCreationModel> : EntityRepository<TEntity, TKey>, IEntityCRDRepository<TEntity, TKey, TCreationModel>
    where TEntity : class, IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected EntityCRDRepository(SocialContext context, IServiceProvider provider) : base(context, provider)
    {
    }

    public abstract ValueTask<SuccessResult<TEntity>> Create(SocialAppUser? requester, TCreationModel model);

    public abstract ValueTask<SuccessResult<object>> GetView(SocialAppUser? requester, TEntity entity);

    public virtual ValueTask<SuccessResult> Delete(SocialAppUser? requester, TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return ValueTask.FromResult(SuccessResult.Success);
    }

    public abstract ValueTask<IQueryable<object>?> GetViews(SocialAppUser? requester, IQueryable<TEntity>? users);
}
