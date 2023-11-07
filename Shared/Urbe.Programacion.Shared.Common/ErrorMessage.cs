using System.Collections;
using System.Net;

namespace Urbe.Programacion.Shared.Common;

public struct ErrorList 
{
    internal List<ErrorMessage>? _errors;

    public readonly int Count => _errors?.Count ?? 0;

    public readonly IEnumerable<ErrorMessage> Errors => _errors ?? (IEnumerable<ErrorMessage>)Array.Empty<ErrorMessage>();

    public HttpStatusCode? RecommendedCode { get; set; }
}

public static class ErrorListExtensions
{
    public static void AddError(this ref ErrorList list, ErrorMessage message)
    {
        (list._errors ??= new()).Add(message);
    }

    public static void AddErrorRange(this ref ErrorList list, IEnumerable<ErrorMessage> messages)
    {
        (list._errors ??= new()).AddRange(messages);
    }
}

public readonly record struct ErrorMessageProperty(string Key, string Value);

public readonly record struct ErrorMessage(string? DefaultMessageES, string Key, IEnumerable<ErrorMessageProperty>? Properties);
