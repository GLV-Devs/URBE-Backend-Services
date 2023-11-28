using System.Text.Json;
using Urbe.Programacion.AppSocial.ClientLibrary;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Data;

public class LoggerSocialApiClient : SocialApiClient
{
    private readonly ILogger Log;

    public LoggerSocialApiClient(HttpClient http, ILogger<SocialApiClient> log, JsonSerializerOptions? options = null) : base(http, options)
    {
        Log = log;
    }

    protected override async Task<SocialApiRequestResponse> HandleResponseMessage(HttpRequestMessage message, CancellationToken ct)
    {
        try
        {
            var r = await base.HandleResponseMessage(message, ct);
            Log.LogDebug("Response Message:\n\tHttpCode: {code}\n\tApiResponseCode: {apicode}\n\tData: {data}\n\tErrors: {errors}\n\tBearer Token Exists: {bte}",
                r.HttpStatusCode,
                r.APIResponse.Code,
                r.APIResponse.Data,
                r.APIResponse.Errors,
                string.IsNullOrWhiteSpace(r.APIResponse.BearerToken) is false
            );
            return r;
        }
        catch(Exception e)
        {
            Log.LogCritical(e, "An error ocurred while requesting: {messageuri}", message.RequestUri?.ToString());
            throw;
        }
    }
}
