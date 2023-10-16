using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly record struct GuidId<TEntity> : IConvertibleProperty, IEquatable<Guid>
    where TEntity : class, IEntity
{
    public Guid Value { get; }

    public GuidId(Guid id)
    {
        Value = id;
    }

    public static implicit operator GuidId<TEntity>(Guid id)
        => new(id);

    public static explicit operator Guid(GuidId<TEntity> id)
        => id.Value;

    public static ValueConverter ValueConverter { get; } = new ValueConverter<GuidId<TEntity>, Guid>(
        x => x.Value,
        x => (GuidId<TEntity>)x
    );

    public bool Equals(Guid other)
        => Value == other;
}