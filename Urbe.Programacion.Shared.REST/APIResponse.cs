using System.Text.Json.Serialization;
using DiegoG.REST;

namespace Urbe.Programacion.Shared.Common;

public class APIResponse<TObjectCode> : RESTObject<TObjectCode>
    where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
{
    public APIResponse(TObjectCode code) : base(code) { }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ErrorMessage>? Errors { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraceId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Exception { get; init; }
}
