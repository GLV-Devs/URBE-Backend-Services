using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages.Identity;

public class LogOutModel : PageModel
{
    private readonly SignInManager<VehicleUser> SignInManager;

    public LogOutModel(SignInManager<VehicleUser> userManager)
    {
        SignInManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await SignInManager.SignOutAsync();
        return Redirect("/");
    }
}
