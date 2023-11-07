using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppVehiculos.Entities.Data;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Implementations;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;
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

    [FromQuery]
    public string? Model { get; set; }

    [FromQuery]
    public string? Make { get; set; }

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

    [FromQuery]
    public long? Delete { get; set; }

    public List<VehicleReportView> VehicleReport { get;set; } = default!;

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
            return Unauthorized();

        await FillReportList(user);
        return Page();
    }

    private async ValueTask FillReportList(VehicleUser user)
    {
        var uid = user.Id;

        var query = VehicleReportRepository.Query().Where(x => x.OwnerId == uid);

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

        if (VehicleMaintenanceType is VehicleMaintenanceType vmt)
            query = query.Where(x => x.MaintenanceType == vmt);

        if (VehicleCountryAlpha3Code is string vca3c)
            query = query.Where(x => x.VehicleCountryAlpha3Code == vca3c);

        if (LicensePlate is string lp)
            query = query.Where(x => x.LicensePlate == lp);

        if (Model is string vm)
            query = query.Where(x => x.VehicleModel == vm);

        //if (Make is string vmk)
        //query = query.Where(x => x.VehicleMake == vmk);

        VehicleReport = await (await VehicleReportRepository.GetViews(user, query))!.Cast<VehicleReportView>().ToListAsync();

    }
}
