using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly record struct GuidId<TEntity> where TEntity : class, IEntity
{
    public Guid Id { get; }

    public GuidId(Guid id)
    {
        Id = id;
    }

    public static implicit operator GuidId<TEntity>(Guid id)
        => new(id);

    public static explicit operator Guid(GuidId<TEntity> id)
        => id.Id;
}