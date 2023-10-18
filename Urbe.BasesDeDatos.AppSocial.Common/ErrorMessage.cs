using System.Collections;
using System.Net;
using System.Text.Json.Serialization;

namespace Urbe.BasesDeDatos.AppSocial.Common;

public struct ErrorList : IEnumerable<ErrorMessage>
{
    internal List<ErrorMessage>? _errors;

    public readonly int Count => _errors?.Count ?? 0;

    public readonly IEnumerable<ErrorMessage> Errors => _errors ?? (IEnumerable<ErrorMessage>)Array.Empty<ErrorMessage>();

    public HttpStatusCode? RecommendedCode { get; set; }

    public readonly IEnumerator<ErrorMessage> GetEnumerator()
        => Errors.GetEnumerator();

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

public readonly record struct ErrorMessageProperty(string Key, string Value);

public readonly record struct ErrorMessage(string? DefaultMessageES, string Key, IEnumerable<ErrorMessageProperty>? Properties);
