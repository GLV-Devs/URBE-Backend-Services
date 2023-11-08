using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppVehiculos.Entities.Data;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Implementations;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;
using Urbe.Programacion.AppVehiculos.WebApp.Data.RouteData;
using Urbe.Programacion.AppVehiculos.WebApp.Pages.Identity;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages.Reports;

public class EditModel : PageModel
{
    protected readonly IVehicleReportRepository VehicleReportRepository;
    protected readonly UserManager<VehicleUser> UserManager;

    public EditModel(IVehicleReportRepository vehicleReportRepository, UserManager<VehicleUser> userManager)
    {
        VehicleReportRepository = vehicleReportRepository ?? throw new ArgumentNullException(nameof(vehicleReportRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [BindProperty]
    public VehicleReportUpdateModel UpdateModel { get; set; } = default!;

    public ErrorList Errors => errorList;
    private ErrorList errorList;

    public VehicleReport Report { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(long rid)
    {
        var id = new Snowflake(rid);

        var vehiclereport = await VehicleReportRepository.Find(id);
        if (vehiclereport is null)
            return NotFound();

        var user = await UserManager.GetUserAsync(User);

        if (user is null)
        {
            TempData[LogInModel.RedirectDestinationTempDataKey] = $"/Reports/Edit/{rid}";
            return RedirectToRoute($"/Identity/LogIn");
        }

        if (await VehicleReportRepository.CanEditReport(user, vehiclereport) is false)
            return Unauthorized();

        Report = vehiclereport;

        UpdateModel = new VehicleReportUpdateModel()
        {
            LicensePlate = vehiclereport.LicensePlate,
            CorrectiveMaintenance = vehiclereport.MaintenanceType.HasFlag(VehicleMaintenanceType.Corrective),
            PreventiveMaintenance = vehiclereport.MaintenanceType.HasFlag(VehicleMaintenanceType.Preventive),
            VehicleColor = vehiclereport.VehicleColor,
            VehicleCountryAlpha3Code = vehiclereport.VehicleCountryAlpha3Code,
            VehicleModel = vehiclereport.VehicleModel,
            VehicleMake = vehiclereport.VehicleMake
        };

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(long rid)
    {
        var id = new Snowflake(rid);

        var vehiclereport = await VehicleReportRepository.Find(id);
        if (vehiclereport is null)
            return NotFound();

        var user = await UserManager.GetUserAsync(User);
        if (user is null || await VehicleReportRepository.CanEditReport(user, vehiclereport) is false)
            return Unauthorized();

        var result = await VehicleReportRepository.Update(user, vehiclereport, UpdateModel);
        if (result.IsSuccess is false)
        {
            errorList.AddErrorRange(result.ErrorMessages.Errors);
            return Page();
        }

        await VehicleReportRepository.SaveChanges();

        return RedirectToPage("./Index");
    }
}
