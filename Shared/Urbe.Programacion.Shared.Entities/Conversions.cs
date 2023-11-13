using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.Shared.Entities;
public static class Conversions
{
    public static ValueConverter SnowflakeValueConverter { get; } = new ValueConverter<Snowflake, long>(
        x => x.AsLong(),
        y => new(y)
    );

    public static ValueConverter RandomKeyValueConverter { get; } = new ValueConverter<RandomKey, byte[]>(
        x => x.ToByteArray(),
        y => new(y)
    );
}
