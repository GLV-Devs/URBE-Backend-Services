using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.WebApp.Server.Options;
using Urbe.Programacion.AppSocial.WebApp.Server.Services;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Authentication;

public class JwtSignInHandler : SignInAuthenticationHandler<JwtSignInHandlerOptions>
{

    private const string HeaderValueNoCache = "no-cache";
    private const string HeaderValueNoCacheNoStore = "no-cache,no-store";
    private const string HeaderValueEpocDate = "Thu, 01 Jan 1970 00:00:00 GMT";
    protected readonly UserManager<SocialAppUser> UserManager;

    public JwtSignInHandler(
        IOptionsMonitor<JwtSignInHandlerOptions> options, 
        UserManager<SocialAppUser> userManager,
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
        UserManager = userManager;
    }

    protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        Response.Headers.CacheControl = HeaderValueNoCacheNoStore;
        Response.Headers.Pragma = HeaderValueNoCache;
        Response.Headers.Expires = HeaderValueEpocDate;

        var u = await UserManager.GetUserAsync(user);
        if (u is null)
        {
            await Context.ChallengeAsync("Identity.Application");
            return;
        }

        var claims = user.Identities.FirstOrDefault();
        if (claims is null)
        {
            await Context.ChallengeAsync("Identity.Application");
            return;
        }

        Context.Features.Set(new JwtBearerState(TokenFactory().CreateAndWriteToken(claims)));
    }

    protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
        => Task.CompletedTask;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        => Context.AuthenticateAsync("Bearer");

    private JwtFactory TokenFactory()
        => Options.TokenFactory ?? throw new InvalidOperationException("Options are invalid as TokenFactory is null");
}
