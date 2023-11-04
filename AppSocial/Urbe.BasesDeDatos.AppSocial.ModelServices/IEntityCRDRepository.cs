using System.Diagnostics.CodeAnalysis;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices;

public interface IEntityCRDRepository<TEntity, TKey, TCreationModel> : IEntityRepository<TEntity, TKey>
    where TEntity : IEntity, IKeyed<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public ValueTask<SuccessResult<TEntity>> Create(SocialAppUser? requester, TCreationModel model);

    public ValueTask<SuccessResult<object>> GetView(SocialAppUser? requester, TEntity entity);

    public ValueTask<IQueryable<object>?> GetViews(SocialAppUser? requester, IQueryable<TEntity>? users);

    public ValueTask<SuccessResult> Delete(SocialAppUser? requester, TEntity entity);
}
