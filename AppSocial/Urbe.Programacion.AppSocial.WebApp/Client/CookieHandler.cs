using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Urbe.Programacion.AppSocial.WebApp.Client;

public class CookieHandler : DelegatingHandler
{
    public CookieHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {

    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include)
               .SetBrowserRequestCache(BrowserRequestCache.NoStore);

        return await base.SendAsync(request, cancellationToken);
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include)
               .SetBrowserRequestCache(BrowserRequestCache.NoStore);

        return base.Send(request, cancellationToken);
    }
}
