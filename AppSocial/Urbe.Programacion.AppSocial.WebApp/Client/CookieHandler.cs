using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Urbe.Programacion.AppSocial.WebApp.Client;

public class CookieHandler : DelegatingHandler
{
    public CookieHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {

    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}
