using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.Programacion.AppSocial.API.Filters;

public sealed class SignInRefreshFilter : IAsyncActionFilter
{
    private SignInManager<SocialAppUser> SignInManager { get; }
    private UserManager<SocialAppUser> UserManager { get; }
    public SignInRefreshFilter(SignInManager<SocialAppUser> signInManager, UserManager<SocialAppUser> userManager)
    {
        SignInManager = signInManager;
        UserManager = userManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ctx = await next();

        var u = await UserManager.GetUserAsync(ctx.HttpContext.User);
        if (u is not null)
            await SignInManager.RefreshSignInAsync(u);
    }
}
