using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public readonly struct RandomKey : IEquatable<RandomKey>, IEqualityOperators<RandomKey, RandomKey, bool>, IFormattable, IParsable<RandomKey>, IConvertibleProperty
{
    private const int LengthInLongs = 8;
    private const int LengthInBytes = LengthInLongs * sizeof(ulong);
    private const int LengthInBits = LengthInBytes * 8;
    private const int Base64Length = 8;

    private readonly ulong A;
    private readonly ulong B;
    private readonly ulong C;
    private readonly ulong D;

    private readonly ulong E;
    private readonly ulong F;
    private readonly ulong G;
    private readonly ulong H;

    public unsafe ReadOnlySpan<ulong> AsSpan()
        => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in A), LengthInLongs);

    public unsafe ReadOnlySpan<byte> AsBytes()
        => MemoryMarshal.AsBytes(AsSpan());

    public ulong[] ToArray()
        => AsSpan().ToArray();

    public byte[] ToByteArray()
        => AsBytes().ToArray();

    public unsafe RandomKey(ReadOnlySpan<byte> array)
    {
        if (array.Length < LengthInBytes)
            throw new ArgumentException("array has too few elements", nameof(array));

        var buf = MemoryMarshal.Cast<byte, ulong>(array);
        fixed (ulong* ptr = &A)
        {
            var buffer = new Span<ulong>(ptr, LengthInLongs);
            for (int i = 0; i < LengthInLongs; i++) ptr[i] = buf[i];
        }
    }

    public unsafe RandomKey(ReadOnlySpan<ulong> array)
    {
        if (array.Length < LengthInLongs)
            throw new ArgumentException("array has too few elements", nameof(array));

        fixed (ulong* ptr = &A)
        {
            var buffer = new Span<ulong>(ptr, LengthInLongs);
            for (int i = 0; i < LengthInLongs; i++) ptr[i] = array[i];
        }
    }

    public RandomKey(ulong a, ulong b, ulong c, ulong d, ulong e, ulong f, ulong g, ulong h)
    {
        A = a;
        B = b;
        C = c;
        D = d;
        E = e;
        F = f;
        G = g;
        H = h;
    }

    public unsafe static RandomKey NewHashKey()
    {
        var hashkey = new RandomKey();
        var longs = new Span<ulong>(&hashkey.A, LengthInLongs);
        var bytes = MemoryMarshal.AsBytes(longs);
        RandomNumberGenerator.Fill(bytes);
        longs[4] = (ulong)DateTime.UtcNow.Ticks;

        return hashkey;
    }

    private static bool EqualsCore(RandomKey a, RandomKey b)
    {
        if (Vector256.IsHardwareAccelerated)
            return Vector256.LoadUnsafe(ref Unsafe.AsRef(in a.A), 0) == Vector256.LoadUnsafe(ref Unsafe.AsRef(in b.A), 0)
                && Vector256.LoadUnsafe(ref Unsafe.AsRef(in a.E), sizeof(ulong) * 4) == Vector256.LoadUnsafe(ref Unsafe.AsRef(in b.E), sizeof(ulong) * 4);

        if (Vector128.IsHardwareAccelerated)
            return Vector128.LoadUnsafe(ref Unsafe.AsRef(in a.A), 0) == Vector128.LoadUnsafe(ref Unsafe.AsRef(in b.A), 0)
                && Vector128.LoadUnsafe(ref Unsafe.AsRef(in a.C), sizeof(ulong) * 2) == Vector128.LoadUnsafe(ref Unsafe.AsRef(in b.C), sizeof(ulong) * 2)
                && Vector128.LoadUnsafe(ref Unsafe.AsRef(in a.E), sizeof(ulong) * 4) == Vector128.LoadUnsafe(ref Unsafe.AsRef(in b.E), sizeof(ulong) * 4)
                && Vector128.LoadUnsafe(ref Unsafe.AsRef(in a.G), sizeof(ulong) * 6) == Vector128.LoadUnsafe(ref Unsafe.AsRef(in b.G), sizeof(ulong) * 6);

        return a.A == b.A && a.B == b.B && a.C == b.C && a.D == b.D &&
            a.E == b.E && a.F == b.F && a.G == b.G && a.H == b.H;
    }

    public bool Equals(RandomKey other)
        => EqualsCore(this, other);

    public override bool Equals(object? obj) 
        => obj is RandomKey key && Equals(key);

    public static bool operator ==(RandomKey left, RandomKey right) 
        => EqualsCore(left, right);

    public static bool operator !=(RandomKey left, RandomKey right)
        => EqualsCore(left, right) is false;

    public override int GetHashCode()
        => HashCode.Combine(A, B, C, D, E, F, G, H);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        Span<byte> str = stackalloc byte[Base64Length];

        var opstatus = Base64.EncodeToUtf8(AsBytes(), str, out _, out _);
        return opstatus is not System.Buffers.OperationStatus.Done
            ? throw new InvalidOperationException($"An error ocurred during formatting; operation status: {opstatus}")
            : Encoding.UTF8.GetString(str);
    }

    public override string ToString()
        => ToString(null, null);

    public static RandomKey Parse(string s, IFormatProvider? provider)
    {
        ArgumentException.ThrowIfNullOrEmpty(s);
        return TryParse(s, null, out var result) ? result : throw new FormatException("String was in an incorrect format");
    }

    public unsafe static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RandomKey result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        var len = Encoding.UTF8.GetByteCount(s);
        Span<byte> bytes = stackalloc byte[len];
        Encoding.UTF8.GetBytes(s, bytes);

        Span<byte> output = stackalloc byte[LengthInBytes];
        if (Base64.DecodeFromUtf8(bytes, output, out _, out _) is not System.Buffers.OperationStatus.Done)
        {
            result = default;
            return false;
        }

        fixed (byte* ptr = output)
            result = new(new Span<ulong>(ptr, LengthInLongs));
        return true;
    }

    public static ValueConverter ValueConverter { get; } = new ValueConverter<RandomKey, byte[]>(
        x => x.ToByteArray(),
        y => new(y)
    );
}
