using System.Net.Http.Json;

using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public abstract class SocialApiClientModule
{
    public SocialApiClient Client { get; }

    public string Controller { get; }

    protected HttpClient Http => Client.Http;

    #region With Data

    protected ApiResponseTask Delete(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse(
            Http.DeleteAsync(
                $"{Controller}/{endpoint}",
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Get(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse(
            Http.GetAsync(
                $"{Controller}/{endpoint}", 
                ct
            ), 
            Client.JsonOptions, 
            ct
        );

    protected ApiResponseTask Put(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse(
            Http.PutAsync(
                $"{Controller}/{endpoint}",
                null,
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Post(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse(
            Http.PostAsync(
                $"{Controller}/{endpoint}",
                null,
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Put<TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse(
            Http.PutAsync(
                $"{Controller}/{endpoint}",
                body is null ? null : JsonContent.Create(body, options: Client.JsonOptions),
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Post<TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse(
            Http.PostAsync(
                $"{Controller}/{endpoint}", 
                body is null ? null : JsonContent.Create(body, options: Client.JsonOptions), 
                ct
            ), 
            Client.JsonOptions, 
            ct
        );

    #endregion

    internal SocialApiClientModule(SocialApiClient client, string controller)
    {
        ArgumentException.ThrowIfNullOrEmpty(controller);

        Client = client ?? throw new ArgumentNullException(nameof(client));
        Controller = controller;
    }
}
