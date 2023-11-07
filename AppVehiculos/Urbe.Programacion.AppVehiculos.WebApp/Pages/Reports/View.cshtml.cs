using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data;
using Urbe.Programacion.Shared.Entities;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages.Reports;

public class ViewModel : PageModel
{
    protected readonly IVehicleReportRepository VehicleReportRepository;
    protected readonly UserManager<VehicleUser> UserManager;

    public ViewModel(IVehicleReportRepository vehicleReportRepository, UserManager<VehicleUser> userManager)
    {
        VehicleReportRepository = vehicleReportRepository ?? throw new ArgumentNullException(nameof(vehicleReportRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public int Pages { get; set; }

    [FromQuery]
    public int PageLength { get; set; } = 8;

    [FromQuery]
    public int PageIndex { get; set; }

    [FromQuery]
    public Snowflake? ReportId { get; set; }

    [FromQuery]
    public string? Model { get; set; }

    [FromQuery]
    public string? LicensePlate { get; set; }

    [FromQuery]
    public string? VehicleCountryAlpha3Code { get; set; }

    [FromQuery]
    public VehicleMaintenanceType? VehicleMaintenanceType { get; set; }

    [FromQuery]
    public uint? VehicleColor { get; set; }

    [FromQuery]
    public float? VehicleColorTolerance { get; set; }

    [FromQuery]
    public DateTimeOffset? CreatedStart { get; set; }

    [FromQuery]
    public DateTimeOffset? CreatedEnd { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
            return Unauthorized();
        throw null;
    }
}
