using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Urbe.Programacion.AppSocial.DataTransfer;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Middleware;

public class SocialAppAuthorizationMiddlewareResultHandler 
{
    private readonly RequestDelegate _next;

    public SocialAppAuthorizationMiddlewareResultHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (_next is not null)
            await _next.Invoke(context);

        if (context.Response.HasStarted is false && context.Response.StatusCode is 401 or 403)
            await context.Response.WriteAsJsonAsync(new BearerAPIResponse(APIResponseCodeEnum.Empty)
            {
                TraceId = context.TraceIdentifier
            });
    }
}