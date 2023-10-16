using System.Collections;
using System.Net;
using System.Text.Json.Serialization;

namespace Urbe.BasesDeDatos.AppSocial.Common;

public struct ErrorList : IEnumerable<ErrorMessage>
{
    internal List<ErrorMessage>? _errors;

    public readonly int Count => _errors?.Count ?? 0;

    [JsonIgnore]
    public HttpStatusCode? RecommendedCode { get; set; }

    public readonly IEnumerator<ErrorMessage> GetEnumerator() 
        => (_errors ?? (IEnumerable<ErrorMessage>)Array.Empty<ErrorMessage>()).GetEnumerator();

    readonly IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}

public static class ErrorListExtensions
{
    public static void AddError(this ref ErrorList list, ErrorMessage message)
    {
        (list._errors ??= new()).Add(message);
    }
}

public readonly record struct ErrorMessage(string Message);
