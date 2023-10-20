using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Urbe.BasesDeDatos.AppSocial.Entities;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.JsonConverters;

public class SnowflakeConverter : JsonConverter<Snowflake>
{
    private SnowflakeConverter() { }

    public static SnowflakeConverter Instance { get; } = new();

    public override Snowflake Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.AsLong());
}
