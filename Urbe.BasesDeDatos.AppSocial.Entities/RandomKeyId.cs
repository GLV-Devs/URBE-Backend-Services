using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly record struct RandomKeyId<TEntity>(RandomKey Value) 
    : IConvertibleProperty, IEquatable<RandomKey>, IEquatable<RandomKeyId<TEntity>>
    where TEntity : class, IEntity
{
    public static ValueConverter ValueConverter { get; } = new ValueConverter<RandomKeyId<TEntity>, byte[]>(
        x => x.Value.ToByteArray(),
        x => new RandomKeyId<TEntity>(new RandomKey(x))
    );

    public bool Equals(RandomKey other)
        => Value == other;
}
