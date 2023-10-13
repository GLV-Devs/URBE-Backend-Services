using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly record struct SnowflakeId<TEntity> where TEntity : class, IEntity
{
    public Snowflake Id { get; }

    public SnowflakeId(Snowflake id)
    {
        Id = id;
    }

    public static implicit operator SnowflakeId<TEntity>(Snowflake id)
        => new(id);

    public static explicit operator Snowflake(SnowflakeId<TEntity> id)
        => id.Id;
}
