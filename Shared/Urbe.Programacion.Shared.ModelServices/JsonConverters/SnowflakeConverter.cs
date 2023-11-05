using System.Text.Json;
using System.Text.Json.Serialization;
using Urbe.Programacion.Shared.Entities;

namespace Urbe.Programacion.Shared.ModelServices.JsonConverters;

public class SnowflakeConverter : JsonConverter<Snowflake>
{
    private SnowflakeConverter() { }

    public static SnowflakeConverter Instance { get; } = new();

    public override Snowflake Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, Snowflake value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value.AsLong());
}
