using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public sealed class SocialApiClient
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
}

