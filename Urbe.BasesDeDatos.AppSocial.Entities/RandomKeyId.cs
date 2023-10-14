using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly struct RandomKeyId<TEntity> : IConvertibleProperty
    where TEntity : class, IEntity
{

}
