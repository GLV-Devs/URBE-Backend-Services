using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

[StructLayout(LayoutKind.Explicit)]
public readonly struct SnowflakeId : IEquatable<SnowflakeId>, IComparable<SnowflakeId>, IParsable<SnowflakeId>, IFormattable
{
    private readonly static long ReferenceStampUtc;

    static SnowflakeId()
    {
        ReferenceStampUtc = 638327520000000000; // October 13, 2023
    }

    private static int LastStamp;
    private static uint LastIndex;

    [FieldOffset(0)]
    private readonly long aslong;

    [FieldOffset(0)]
    private readonly int timeStamp;

    [FieldOffset(sizeof(int))]
    private readonly uint index;

    public SnowflakeId(int timeStamp, uint index) 
    {
        this.timeStamp = timeStamp;
        this.index = index;
    }

    public SnowflakeId(long value)
    {
        aslong = value;
    }

    public static int GetSnowflakeTimeStamp()
        => (int)(DateTime.UtcNow.Ticks - ReferenceStampUtc);

    public static SnowflakeId New()
    {
        var stamp = GetSnowflakeTimeStamp();
        if (stamp != LastStamp)
        {
            LastIndex = 0;
            LastStamp = stamp;
        }

        return new SnowflakeId(stamp, LastIndex++);
    }

    public long AsLong() => aslong;
    public DateTime TimeStamp => new(timeStamp + ReferenceStampUtc, DateTimeKind.Utc);
    public uint Index => index;

    public bool Equals(SnowflakeId other)
        => aslong == other.aslong;

    public int CompareTo(SnowflakeId other)
        => aslong.CompareTo(other.aslong);

    public static SnowflakeId Parse(string s, IFormatProvider? provider)
        => new(long.Parse(s, provider));

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SnowflakeId result)
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

    public static bool operator ==(SnowflakeId left, SnowflakeId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SnowflakeId left, SnowflakeId right)
    {
        return !(left == right);
    }

    public static bool operator <(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) >= 0;
    }
}
