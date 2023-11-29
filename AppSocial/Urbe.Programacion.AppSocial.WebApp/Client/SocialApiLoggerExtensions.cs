using Urbe.Programacion.AppSocial.ClientLibrary;

namespace Urbe.Programacion.AppSocial.WebApp.Client;

public static class SocialApiLoggerExtensions
{
    public static void LogRequestResponse(this ILogger logger, SocialApiRequestResponse r)
    {
        logger.LogInformation("Response Message:\n\tHttpCode: {code}\n\tApiResponseCode: {apicode}\n\tData: {data}\n\tErrors: {errors}\n\tBearer Token Exists: {bte}",
                r.HttpStatusCode,
                r.APIResponse.Code,
                r.APIResponse.Data,
                r.APIResponse.Errors,
                string.IsNullOrWhiteSpace(r.APIResponse.BearerToken) is false
            );

        if (r.APIResponse.Errors?.Any() is true)
            foreach (var error in r.APIResponse.Errors)
                logger.LogError("Error: Key: {key}, Description: {desc}", error.Key, error.DefaultMessageES);
    }
}
