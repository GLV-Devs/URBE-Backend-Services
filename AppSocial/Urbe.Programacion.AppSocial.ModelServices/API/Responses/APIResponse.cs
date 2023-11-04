﻿using System.Text.Json.Serialization;
using DiegoG.REST;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.ModelServices.API.Responses;

public class APIResponse : RESTObject<SocialAPIResponseCode>
{
    public APIResponse(SocialAPIResponseCode code) : base(code) { }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ErrorMessage>? Errors { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TraceId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Exception { get; init; }
}
