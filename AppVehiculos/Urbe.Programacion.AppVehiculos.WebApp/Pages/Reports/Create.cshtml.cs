using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Urbe.Programacion.AppVehiculos.Entities.Data;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;
using Urbe.Programacion.AppVehiculos.WebApp.Data.RouteData;
using Urbe.Programacion.AppVehiculos.WebApp.Pages.Identity;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages.Reports;

public class CreateModel : PageModel
{
    protected readonly IVehicleReportRepository VehicleReportRepository;
    protected readonly UserManager<VehicleUser> UserManager;

    public CreateModel(IVehicleReportRepository vehicleReportRepository, UserManager<VehicleUser> userManager)
    {
        VehicleReportRepository = vehicleReportRepository ?? throw new ArgumentNullException(nameof(vehicleReportRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [BindProperty]
    public VehicleReportUpdateModel UpdateModel { get; set; } = default!;

    public ErrorList Errors => errorList;
    private ErrorList errorList;

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            TempData[LogInModel.RedirectDestinationTempDataKey] = "/Reports/Create";
            return RedirectToRoute($"/Identity/LogIn");
        }
        else
        {
            return Page();
        }
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
            return Unauthorized();

        if (UpdateModel is null)
            return BadRequest();

        var result = await VehicleReportRepository.Create(user, UpdateModel);
        if (result.IsSuccess is false)
        {
            errorList.AddErrorRange(result.ErrorMessages.Errors);
            return Page();
        }

        await VehicleReportRepository.SaveChanges();

        return RedirectToPage("./Index");
    }
}
