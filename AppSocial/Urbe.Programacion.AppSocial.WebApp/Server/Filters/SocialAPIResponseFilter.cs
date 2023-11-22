using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Filters;
using Urbe.Programacion.AppSocial.DataTransfer;
using Urbe.Programacion.AppSocial.WebApp.Server.Authentication;
using Urbe.Programacion.Shared.API.Common.Filters;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Filters;

public class SocialAPIResponseFilter : APIResponseFilter<SocialAPIResponseCode>
{
    public static SocialAPIResponseFilter SocialInstance { get; } = new();

    protected override BearerAPIResponse CreateAPIResponseObject(SocialAPIResponseCode code, ResultExecutingContext context)
    {
        var token = context.HttpContext.Features.Get<JwtBearerState>();
        return new(code)
        {
            BearerToken = token?.Token
        };
    }
}
