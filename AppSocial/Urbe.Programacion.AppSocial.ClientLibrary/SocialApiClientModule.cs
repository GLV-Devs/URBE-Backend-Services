using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public abstract class SocialApiClientModule
{
    private readonly static MediaTypeHeaderValue JsonMediaType = new("application/json");

    public SocialApiClient Client { get; }

    public string Controller { get; }

    protected HttpClient Http => Client.Http;

    #region With Data

    protected ApiResponseTask SendMessage<T>(HttpMethod method, string? endpoint, T body, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        using var msg = new HttpRequestMessage(method, new Uri($"{Controller}/{endpoint}", UriKind.RelativeOrAbsolute));

        msg.Content = JsonContent.Create(body);

        try
        {
            return SocialApiRequestResponse.FromResponse(Http.SendAsync(msg, ct), Client.JsonOptions, ct);
        }
        catch (Exception e)
        {
            throw new SocialApiClientException(endpoint, method.Method, body, "An error ocurred while performing a request to the API", e);
        }
    }

    protected ApiResponseTask SendMessage(HttpMethod method, string? endpoint, CancellationToken ct = default)
    {
        using var msg = new HttpRequestMessage(method, new Uri($"{Controller}/{endpoint}", UriKind.RelativeOrAbsolute));

        try
        {
            return SocialApiRequestResponse.FromResponse(Http.SendAsync(msg, ct), Client.JsonOptions, ct);
        }
        catch(Exception e)
        {
            throw new SocialApiClientException(endpoint, method.Method, null, "An error ocurred while performing a request to the API", e);
        }
    }

    protected ApiResponseTask Delete(string? endpoint, CancellationToken ct = default)
        => SendMessage(HttpMethod.Delete, endpoint, ct);

    protected ApiResponseTask Get(string? endpoint, CancellationToken ct = default)
        => SendMessage(HttpMethod.Get, endpoint, ct);

    protected ApiResponseTask Put(string? endpoint, CancellationToken ct = default)
        => SendMessage(HttpMethod.Put, endpoint, ct);

    protected ApiResponseTask Post(string? endpoint, CancellationToken ct = default)
        => SendMessage(HttpMethod.Post, endpoint, ct);

    protected ApiResponseTask Put<TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => SendMessage(HttpMethod.Put, endpoint, body, ct);

    protected ApiResponseTask Post<TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => SendMessage(HttpMethod.Post, endpoint, body, ct);

    #endregion

    internal SocialApiClientModule(SocialApiClient client, string controller)
    {
        ArgumentException.ThrowIfNullOrEmpty(controller);

        Client = client ?? throw new ArgumentNullException(nameof(client));
        Controller = controller;
    }
}
