﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities;

[StructLayout(LayoutKind.Explicit)]
public readonly struct Snowflake : IEquatable<Snowflake>, IComparable<Snowflake>, IParsable<Snowflake>, IFormattable, IConvertibleProperty
{
    private static readonly long ReferenceStampUtc;

    static Snowflake()
    {
        ReferenceStampUtc = 638327520000000000; // October 13, 2023
    }

    public static ushort SnowflakeMachineId { get; set; }

    private static int LastStamp;
    private static ushort LastIndex;

    [FieldOffset(0)]
    private readonly long aslong;

    [FieldOffset(0)]
    private readonly int timeStamp;

    [FieldOffset(sizeof(int))]
    private readonly ushort index;

    [FieldOffset(sizeof(int) + sizeof(ushort))]
    private readonly ushort machineId;

    public Snowflake(int timeStamp, ushort index, ushort machineId)
    {
        this.timeStamp = timeStamp;
        this.index = index;
        this.machineId = machineId;
    }

    public Snowflake(long value)
    {
        aslong = value;
    }

    public static int GetSnowflakeTimeStamp()
        => (int)(DateTime.UtcNow.Ticks - ReferenceStampUtc);

    public static Snowflake New()
    {
        var stamp = GetSnowflakeTimeStamp();
        if (stamp != LastStamp)
        {
            LastIndex = 0;
            LastStamp = stamp;
        }

        return new Snowflake(stamp, LastIndex++, SnowflakeMachineId);
    }

    public long AsLong() => aslong;

    public DateTime TimeStamp => new(timeStamp + ReferenceStampUtc, DateTimeKind.Utc);

    public ushort Index => index;

    public ushort MachineId => machineId;

    public bool Equals(Snowflake other)
        => aslong == other.aslong;

    public int CompareTo(Snowflake other)
        => aslong.CompareTo(other.aslong);

    public static Snowflake Parse(string s, IFormatProvider? provider)
        => new(long.Parse(s, provider));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Snowflake result)
    {
        if (long.TryParse(s, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
        => aslong.ToString(format, formatProvider);

    public override string ToString()
        => ToString(null, null);

    public override bool Equals(object? obj)
        => aslong.Equals(obj);

    public override int GetHashCode()
        => aslong.GetHashCode();

    public static bool operator ==(Snowflake left, Snowflake right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Snowflake left, Snowflake right)
    {
        return !(left == right);
    }

    public static bool operator <(Snowflake left, Snowflake right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(Snowflake left, Snowflake right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(Snowflake left, Snowflake right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(Snowflake left, Snowflake right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static ValueConverter ValueConverter { get; } = new ValueConverter<Snowflake, long>(
        x => x.AsLong(),
        y => new(y)
    );
}
