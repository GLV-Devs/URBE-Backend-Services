using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly record struct SnowflakeId<TEntity> : IConvertibleProperty
    where TEntity : class, IEntity
{
    public Snowflake Value { get; }

    public SnowflakeId(Snowflake id)
    {
        Value = id;
    }

    public static implicit operator SnowflakeId<TEntity>(Snowflake id)
        => new(id);

    public static explicit operator Snowflake(SnowflakeId<TEntity> id)
        => id.Value;

    public static ValueConverter ValueConverter { get; } = new ValueConverter<SnowflakeId<TEntity>, long>(
        x => x.Value.AsLong(),
        x => new SnowflakeId<TEntity>(new Snowflake(x))
    );
}
