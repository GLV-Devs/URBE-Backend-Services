using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public class SocialApiClient
{
    internal readonly HttpClient Http;
    internal readonly JsonSerializerOptions? JsonOptions;

    public static JsonSerializerOptions DefaultJsonOptions { get; }
        = new JsonSerializerOptions()
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

    public AuthenticationHeaderValue? Authorization
    {
        set => Http.DefaultRequestHeaders.Authorization = value;
        get => Http.DefaultRequestHeaders.Authorization;
    }

    public SocialApiIdentityClient Identity { get; }
    public SocialApiPostClient Posts { get; }
    public SocialApiUserClient Users { get; }
    public SocialApiVerificationClient Verifications { get; }

    public SocialApiClient(HttpClient http, JsonSerializerOptions? options = null)
    {
        Http = http ?? throw new ArgumentNullException(nameof(http));
        JsonOptions = options ?? DefaultJsonOptions;
        Http.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));

        Identity = new(this);
        Posts = new(this);
        Users = new(this);
        Verifications = new(this);
    }

    public SocialApiClient(string host, JsonSerializerOptions? options = null)
        : this(new HttpClient() { BaseAddress = new Uri(host) }, options) { }

    public SocialApiClient(Uri host, JsonSerializerOptions? options = null)
        : this(new HttpClient() { BaseAddress = host }, options) { }

    protected internal virtual async ApiResponseTask HandleResponseMessage(HttpRequestMessage message, CancellationToken ct)
    {
        var resp = await SocialApiRequestResponse.FromResponse(
            Http.SendAsync(
                message,
                HttpCompletionOption.ResponseHeadersRead,
                ct
            ),
            JsonOptions,
            ct
        );

        if (string.IsNullOrWhiteSpace(resp.APIResponse?.BearerToken) is false)
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resp.APIResponse.BearerToken);

        return resp;
    }

    protected internal virtual ApiResponseTask SendMessage<T>(HttpMethod method, string controller, string? endpoint, T body, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        var msg = new HttpRequestMessage(method, new Uri($"{controller}/{endpoint}", UriKind.RelativeOrAbsolute))
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

    protected internal virtual ApiResponseTask SendMessage(HttpMethod method, string controller, string? endpoint, CancellationToken ct = default)
    {
        var msg = new HttpRequestMessage(method, new Uri($"{controller}/{endpoint}", UriKind.RelativeOrAbsolute));

        try
        {
            return HandleResponseMessage(msg, ct);
        }
        catch (Exception e)
        {
            throw new SocialApiClientException(endpoint, method.Method, null, "An error ocurred while performing a request to the API", e);
        }
    }
}

