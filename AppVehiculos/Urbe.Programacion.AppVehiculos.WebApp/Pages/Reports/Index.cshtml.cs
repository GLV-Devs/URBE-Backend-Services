using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Urbe.Programacion.AppVehiculos.Entities.Data;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Implementations;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;
using Urbe.Programacion.AppVehiculos.WebApp.Data.RouteData;
using Urbe.Programacion.AppVehiculos.WebApp.Pages.Identity;
using Urbe.Programacion.Shared.Common.Localization;
using Urbe.Programacion.Shared.Entities;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages.Reports;

public class IndexModel : PageModel
{
    protected readonly IVehicleReportRepository VehicleReportRepository;
    protected readonly UserManager<VehicleUser> UserManager;

    public IndexModel(IVehicleReportRepository vehicleReportRepository, UserManager<VehicleUser> userManager)
    {
        VehicleReportRepository = vehicleReportRepository ?? throw new ArgumentNullException(nameof(vehicleReportRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [FromForm, FromQuery]
    public string? Model { get; set; }

    [FromForm, FromQuery]
    public string? Make { get; set; }

    [FromForm, FromQuery]
    public string? LicensePlate { get; set; }

    [FromForm, FromQuery]
    public string? VehicleCountryAlpha3Code { get; set; }

    [FromForm, FromQuery]
    public bool CorrectiveMaintenance { get; set; }

    [FromForm, FromQuery]
    public bool PreventiveMaintenance { get; set; }

    [FromForm, FromQuery]
    public uint? VehicleColor { get; set; }

    [FromForm, FromQuery]
    public float? VehicleColorTolerance { get; set; }

    [FromForm, FromQuery]
    public DateTimeOffset? CreatedStart { get; set; }

    [FromForm, FromQuery]
    public DateTimeOffset? CreatedEnd { get; set; }

    [FromForm]
    public long? Delete { get; set; }

    public List<VehicleReportView> VehicleReport { get;set; } = default!;

    public IEnumerable<(VehicleReportView Left, VehicleReportView? Right)> BuildReportRows()
    {
        for (int i = 0; ;) 
        {
            VehicleReportView? l, r;
            r = null;

            if (i < VehicleReport.Count)
                l = VehicleReport[i++];
            else
                break;

            if (i < VehicleReport.Count)
                r = VehicleReport[i++];

            yield return (l, r);
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
            return Unauthorized();

        if (Delete is long del)
        {
            var id = new Snowflake(del);
            var entity = await VehicleReportRepository.Find(id);

            if (entity is null)
                return NotFound();

            if (await VehicleReportRepository.CanEditReport(user, id) is false)
                return Unauthorized();

            await VehicleReportRepository.Delete(user, entity);
            await VehicleReportRepository.SaveChanges();
        }

        await FillReportList(user);
        return Page();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user is null)
        {
            TempData[LogInModel.RedirectDestinationTempDataKey] = "/Reports/Index";
            return RedirectToPage($"/Identity/LogIn");
        }

        await FillReportList(user);
        return Page();
    }

    private async ValueTask FillReportList(VehicleUser user)
    {
        var uid = user.Id;

        var query = VehicleReportRepository.Query();

        if (string.Equals(user.Email, "admin@admin.com", StringComparison.OrdinalIgnoreCase) is false)
            query = query.Where(x => x.OwnerId == uid);

        if (CreatedStart is DateTimeOffset cs)
            query = query.Where(x => x.CreatedDate >= cs);

        if (CreatedEnd is DateTimeOffset ce)
            query = query.Where(x => x.CreatedDate <= ce);

        if (VehicleColor is uint vc)
        {
            var t = VehicleColorTolerance ?? .2f;
            var perc = (uint)(vc * float.Clamp(float.Abs(t), 0, 1));
            if (t is not 1)
                if (t is 0)
                    query = query.Where(x => x.VehicleColor == vc);
                else
                {
                    var upper = t + perc;
                    var lower = t - perc;
                    query = query.Where(x => x.VehicleColor >= lower && x.VehicleColor <= upper);
                }
        }

        if (PreventiveMaintenance ^ CorrectiveMaintenance)
            if (PreventiveMaintenance)
                query = query.Where(x => x.MaintenanceType.HasFlag(VehicleMaintenanceType.Preventive));
            else if (CorrectiveMaintenance)
                query = query.Where(x => x.MaintenanceType.HasFlag(VehicleMaintenanceType.Corrective));

        if (VehicleCountryAlpha3Code is string vca3c)
            query = query.Where(x => x.VehicleCountryAlpha3Code == vca3c);

        if (LicensePlate is string lp)
            query = query.Where(x => x.LicensePlate == lp);

        if (Model is string vm)
            query = query.Where(x => x.VehicleModel == vm);

        if (Make is string vmk)
            query = query.Where(x => x.VehicleMake == vmk);

        VehicleReport = await (await VehicleReportRepository.GetViews(user, query))!.Cast<VehicleReportView>().ToListAsync();

    }
}
