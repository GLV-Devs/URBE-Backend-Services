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

    protected ApiResponseTask Delete(string? endpoint, CancellationToken ct = default)
        => Client.SendMessage(HttpMethod.Delete, Controller, endpoint, ct);

    protected ApiResponseTask Get(string? endpoint, CancellationToken ct = default)
        => Client.SendMessage(HttpMethod.Get, Controller, endpoint, ct);

    protected ApiResponseTask Put(string? endpoint, CancellationToken ct = default)
        => Client.SendMessage(HttpMethod.Put, Controller, endpoint, ct);

    protected ApiResponseTask Post(string? endpoint, CancellationToken ct = default)
        => Client.SendMessage(HttpMethod.Post, Controller, endpoint, ct);

    protected ApiResponseTask Put<TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => Client.SendMessage(HttpMethod.Put, Controller, endpoint, body, ct);

    protected ApiResponseTask Post<TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => Client.SendMessage(HttpMethod.Post, Controller, endpoint, body, ct);

    #endregion

    internal SocialApiClientModule(SocialApiClient client, string controller)
    {
        ArgumentException.ThrowIfNullOrEmpty(controller);

        Client = client ?? throw new ArgumentNullException(nameof(client));
        Controller = controller;
    }
}
