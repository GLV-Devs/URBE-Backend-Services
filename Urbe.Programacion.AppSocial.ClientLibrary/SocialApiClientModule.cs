using System.Net.Http.Json;

using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public abstract class SocialApiClientModule
{
    public SocialApiClient Client { get; }

    public string Controller { get; }

    protected HttpClient Http => Client.Http;

    #region Without Data

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

    #region With Data

    protected ApiResponseTask Delete<T>(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse<T>(
            Http.DeleteAsync(
                $"{Controller}/{endpoint}",
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Get<T>(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse<T>(
            Http.GetAsync(
                $"{Controller}/{endpoint}", 
                ct
            ), 
            Client.JsonOptions, 
            ct
        );

    protected ApiResponseTask Put<T>(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse<T>(
            Http.PutAsync(
                $"{Controller}/{endpoint}",
                null,
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Post<T>(string? endpoint, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse<T>(
            Http.PostAsync(
                $"{Controller}/{endpoint}",
                null,
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Put<TData, TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse<TData>(
            Http.PutAsync(
                $"{Controller}/{endpoint}",
                body is null ? null : JsonContent.Create(body, options: Client.JsonOptions),
                ct
            ),
            Client.JsonOptions,
            ct
        );

    protected ApiResponseTask Post<TData, TBody>(string? endpoint, TBody? body, CancellationToken ct = default)
        => SocialApiRequestResponse.FromResponse<TData>(
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
