namespace Urbe.Programacion.AppSocial.ClientLibrary;

[Serializable]
public class SocialApiClientException : Exception
{
    public string? RequestUri { get; }
    public string? HttpMethod { get; }
    public object? Body { get; }

    public SocialApiClientException(string? requestUri, string? httpMethod, object? body, string message) : base(message) 
    {
        RequestUri = requestUri;
        HttpMethod = httpMethod;
        Body = body;
    }

    public SocialApiClientException(string? requestUri, string? httpMethod, object? body, string message, Exception inner) : base(message, inner) 
    {
        RequestUri = requestUri;
        HttpMethod = httpMethod;
        Body = body;
    }

    protected SocialApiClientException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
