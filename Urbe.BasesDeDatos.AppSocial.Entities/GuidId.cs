using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly record struct GuidId<TEntity> : IConvertibleProperty, IEquatable<Guid>, IParsable<GuidId<TEntity>>, ISpanParsable<GuidId<TEntity>>
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

    public static GuidId<TEntity> Parse(string s, IFormatProvider? provider)
        => Guid.Parse(s, provider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out GuidId<TEntity> result)
    {
        if (Guid.TryParse(s, provider, out Guid guid))
        {
            result = new GuidId<TEntity>(guid);
            return true;
        }

        result = default;
        return false;
    }

    public static GuidId<TEntity> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => Guid.Parse(s, provider);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out GuidId<TEntity> result)
    {
        if (Guid.TryParse(s, provider, out Guid guid))
        {
            result = new GuidId<TEntity>(guid);
            return true;
        }

        result = default;
        return false;
    }
}