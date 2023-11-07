using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Identity;
using Urbe.Programacion.Shared.API.Common;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages.Identity;

public class LogInModel : PageModel
{
    public const string RedirectDestinationQuery = "redirect_destination";

    protected ErrorList errorList;

    public ErrorList Errors => errorList;

    protected readonly SignInManager<VehicleUser> SignInManager;
    protected readonly UserManager<VehicleUser> UserManager;

    public LogInModel(SignInManager<VehicleUser> signInManager, UserManager<VehicleUser> userManager)
    {
        SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public virtual async Task<IActionResult> OnGet()
    {
        var loggedUser = await UserManager.GetUserAsync(User);
        return loggedUser is not null ? ReturnToDestination() : Page();
    }

    public virtual async Task<IActionResult> OnPost()
    {
        HttpStatusCode code;
        var loggedUser = await UserManager.GetUserAsync(User);
        if (loggedUser is not null)
            return ReturnToDestination();

        if (Request.Form.BindThroughConstructor<LogInRequest>(out var login, out _))
        {
            loggedUser = await UserManager.FindByNameAsync(login.UserName) ?? await UserManager.FindByEmailAsync(login.UserName);
            if (loggedUser is null)
            {
                errorList.AddError(ErrorMessages.UserNotFound(login.UserName));
                code = HttpStatusCode.NotFound;
            }
            else
            {
                var result = await SignInManager.PasswordSignInAsync(loggedUser, login.Password, true, false);
                if (result.IsLockedOut)
                {
                    errorList.AddError(ErrorMessages.LoginLockedOut(login.UserName));
                    code = HttpStatusCode.Forbidden;
                }
                else if (result.IsNotAllowed)
                {
                    errorList.AddError(ErrorMessages.ActionDisallowed("Iniciar Sesión"));
                    code = HttpStatusCode.Forbidden;
                }
                else if (result.Succeeded)
                    return ReturnToDestination();
                else
                {
                    errorList.AddError(ErrorMessages.BadPassword());
                    code = HttpStatusCode.Unauthorized;
                }
            }
        }
        else
        {
            errorList.AddError(ErrorMessages.BadLogin());
            code = HttpStatusCode.BadRequest;
        }

        var p = Page();
        p.StatusCode = (int)code;
        return p;
    }

    protected IActionResult ReturnToDestination() 
        => Redirect(Request.Query[RedirectDestinationQuery].FirstOrDefault() ?? "/");
}
