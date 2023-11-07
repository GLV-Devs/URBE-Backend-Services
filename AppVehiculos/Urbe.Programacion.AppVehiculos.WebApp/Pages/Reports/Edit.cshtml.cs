using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data;

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

    public void OnGet()
    {
    }
}
