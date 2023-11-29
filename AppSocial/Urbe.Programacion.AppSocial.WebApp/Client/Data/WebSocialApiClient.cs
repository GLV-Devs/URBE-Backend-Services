using System.Text.Json;
using Urbe.Programacion.AppSocial.ClientLibrary;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Data;

public class WebSocialApiClient : SocialApiClient
{
    private readonly ILogger Log;
    private readonly AppState State;

    public WebSocialApiClient(AppState state, HttpClient http, ILogger<SocialApiClient> log, JsonSerializerOptions? options = null) : base(http, options)
    {
        Log = log;
        State = state;
    }

    protected override async Task<SocialApiRequestResponse> HandleResponseMessage(HttpRequestMessage message, CancellationToken ct)
    {
        try
        {
            var r = await base.HandleResponseMessage(message, ct);
            Log.LogRequestResponse(r);
            if (string.IsNullOrWhiteSpace(r.APIResponse?.BearerToken) is false)
                await State.SetToken(r.APIResponse.BearerToken);
            return r;
        }
        catch(Exception e)
        {
            Log.LogCritical(e, "An error ocurred while requesting: {messageuri}", message.RequestUri?.ToString());
            throw;
        }
    }
}
