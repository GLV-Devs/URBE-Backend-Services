using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public abstract class SocialApiClientModule
{
    public SocialApiClient Client { get; }

    public string Controller { get; }

    protected HttpClient Http => Client.Http;

    #region With Data

    private async ApiResponseTask HandleResponseMessage(HttpRequestMessage message, CancellationToken ct)
    {
        var resp = await SocialApiRequestResponse.FromResponse(
            Http.SendAsync(
                message, 
                HttpCompletionOption.ResponseHeadersRead, 
                ct
            ), 
            Client.JsonOptions, 
            ct
        );

        if (string.IsNullOrWhiteSpace(resp.APIResponse?.BearerToken) is false)
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resp.APIResponse.BearerToken);
        
        return resp;
    }

    protected ApiResponseTask SendMessage<T>(HttpMethod method, string? endpoint, T body, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        var msg = new HttpRequestMessage(method, new Uri($"{Controller}/{endpoint}", UriKind.RelativeOrAbsolute))
        {
            Content = JsonContent.Create(body)
        };

        try
        {
            return HandleResponseMessage(msg, ct);
        }
        catch (Exception e)
        {
            throw new SocialApiClientException(endpoint, method.Method, body, "An error ocurred while performing a request to the API", e);
        }
    }

    protected ApiResponseTask SendMessage(HttpMethod method, string? endpoint, CancellationToken ct = default)
    {
        var msg = new HttpRequestMessage(method, new Uri($"{Controller}/{endpoint}", UriKind.RelativeOrAbsolute));

        try
        {
            return HandleResponseMessage(msg, ct);
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
