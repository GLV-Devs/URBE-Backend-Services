using System.Diagnostics;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Urbe.Programacion.AppVehiculos.Entities.Data;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Settings;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities;
using Urbe.Programacion.Shared.Entities.Models;
using Urbe.Programacion.Shared.ModelServices;
using Urbe.Programacion.Shared.ModelServices.Implementations;
using Urbe.Programacion.Shared.Services.Attributes;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Implementations;

[RegisterService(typeof(IVehicleReportRepository))]
[RegisterService(typeof(IEntityCRUDRepository<VehicleReport, Snowflake, VehicleReportUpdateModel, VehicleReportUpdateModel>))]
public class VehicleReportRepository
    : EntityCRUDRepository<VehicleReport, Snowflake, VehicleReportUpdateModel, VehicleReportUpdateModel>, IVehicleReportRepository
{
    protected readonly VehicleReportSettings VehicleReportSettings;
    protected readonly new VehicleContext context;

    public VehicleReportRepository(
        IOptionsSnapshot<VehicleReportSettings> vehicleReportSettings,
        VehicleContext context,
        IServiceProvider services)
        : base(context, services)
    {
        this.context = context;
        VehicleReportSettings = (vehicleReportSettings ?? throw new ArgumentNullException(nameof(vehicleReportSettings))).Value;
    }

    public override ValueTask<SuccessResult> Update(BaseAppUser? requester, VehicleReport entity, VehicleReportUpdateModel update)
    {
        ErrorList errors = new();

        if (DateTimeOffset.Now >= (entity.CreatedDate + VehicleReportSettings.VehicleReportEditingWindow))
            errors.AddError(ErrorMessages.TimedOut("Editar Reporte de Vehiculo"));

        if (Helper.IsUpdatingString(entity.VehicleModel!, update.VehicleModel))
            Helper.IsTooLong(ref errors, update.VehicleModel, VehicleReport.VehicleDataMaxLength, "Modelo");

        if (Helper.IsUpdatingString(entity.LicensePlate!, update.LicensePlate))
            Helper.IsTooLong(ref errors, update.LicensePlate, VehicleReport.VehicleDataMaxLength, "Placa");

        if (Helper.IsUpdatingString(entity.VehicleCountryAlpha3Code!, update.VehicleCountryAlpha3Code))
            Helper.IsTooLong(ref errors, update.VehicleCountryAlpha3Code, 3, "Codigo de Pais Alfa-3");

        if (Helper.IsUpdating(entity.MaintenanceType, update.MaintenanceType) && update.MaintenanceType.Value.IsValid() is false)
            errors.AddError(ErrorMessages.InvalidProperty("Tipo de Mantenimiento"));

        errors.RecommendedCode = System.Net.HttpStatusCode.BadRequest;
        if (errors.Count > 0)
            return ValueTask.FromResult(new SuccessResult(errors));
        
        if (string.IsNullOrWhiteSpace(update.VehicleModel) is false)
            entity.VehicleModel = update.VehicleModel;

        if (string.IsNullOrWhiteSpace(update.LicensePlate) is false)
            entity.LicensePlate = update.LicensePlate;

        if (string.IsNullOrWhiteSpace(update.VehicleCountryAlpha3Code) is false)
            entity.VehicleCountryAlpha3Code = update.VehicleCountryAlpha3Code;

        if (update.MaintenanceType is VehicleMaintenanceType mt)
            entity.MaintenanceType = mt;

        if (update.VehicleColor is uint color)
            entity.VehicleColor = color;

        return ValueTask.FromResult(SuccessResult.Success);
    }

    public override ValueTask<SuccessResult<VehicleReport>> Create(BaseAppUser? r, VehicleReportUpdateModel model)
    {
        ErrorList errors = new();

        if (Helper.IsEmpty(ref errors, model.VehicleModel, "Modelo") is false)
            Helper.IsTooLong(ref errors, model.VehicleModel, VehicleReport.VehicleDataMaxLength, "Modelo");

        if (Helper.IsEmpty(ref errors, model.LicensePlate, "Placa") is false)
            Helper.IsTooLong(ref errors, model.LicensePlate, VehicleReport.VehicleDataMaxLength, "Placa");

        if (Helper.IsEmpty(ref errors, model.VehicleCountryAlpha3Code, "Codigo de Pais Alfa-3") is false)
            Helper.IsTooLong(ref errors, model.VehicleCountryAlpha3Code, 3, "Codigo de Pais Alfa-3");

        if (model.MaintenanceType is not VehicleMaintenanceType mt || mt.IsValid() is false)
            errors.AddError(ErrorMessages.InvalidProperty("Tipo de Mantenimiento"));

        if (model.VehicleColor is null)
            errors.AddError(ErrorMessages.InvalidProperty("Color de Vehiculo"));

        errors.RecommendedCode = System.Net.HttpStatusCode.BadRequest;
        if (errors.Count > 0)
            return ValueTask.FromResult(new SuccessResult<VehicleReport>(errors));

        var requester = (VehicleUser?)r;
        Debug.Assert(requester is not null);
        Debug.Assert(model.MaintenanceType is not null);

        var n = new VehicleReport()
        {
            Id = Snowflake.New(),
            LicensePlate = model.LicensePlate,
            MaintenanceType = model.MaintenanceType.Value,
            Owner = requester,
            VehicleColor = model.VehicleColor,
            VehicleCountryAlpha3Code = model.VehicleCountryAlpha3Code,
            VehicleModel = model.VehicleModel
        };

        context.Reports.Add(n);

        return ValueTask.FromResult(new SuccessResult<VehicleReport>(n));
    }

    public override async ValueTask<SuccessResult<object>> GetView(BaseAppUser? r, VehicleReport entity)
    {
        if (r is null || r is not VehicleUser requester)
            return SuccessResult<object>.Failure;

        var manager = provider.GetRequiredService<UserManager<VehicleUser>>();
        return requester.Id != entity.OwnerId || await manager.IsInRoleAsync(requester, VehicleUserRole.AdminReportViewerRole) is false
            ? SuccessResult<object>.Failure
            : new SuccessResult<object>(new VehicleReportView(
            entity.Id,
            entity.OwnerId,
            entity.VehicleModel!,
            entity.LicensePlate!,
            entity.VehicleCountryAlpha3Code!,
            entity.MaintenanceType,
            entity.VehicleColor,
            entity.LastModified,
            entity.CreatedDate
        ));
    }

    public override async ValueTask<IQueryable<object>?> GetViews(BaseAppUser? r, IQueryable<VehicleReport>? users)
    {
        if (r is null || r is not VehicleUser requester || users is null)
            return null;
        var reqid = requester.Id;

        var manager = provider.GetRequiredService<UserManager<VehicleUser>>();
        return await manager.IsInRoleAsync(requester, VehicleUserRole.AdminReportViewerRole) is false
            ? users.AsNoTracking().OrderBy(x => x.OwnerId).Select(x => new VehicleReportView(
                x.Id,
                x.OwnerId,
                x.VehicleModel!,
                x.LicensePlate!,
                x.VehicleCountryAlpha3Code!,
                x.MaintenanceType,
                x.VehicleColor,
                x.LastModified,
                x.CreatedDate
            ))
            : users.AsNoTracking().Where(x => reqid == x.OwnerId).Select(x => new VehicleReportView(
                x.Id,
                x.OwnerId,
                x.VehicleModel!,
                x.LicensePlate!,
                x.VehicleCountryAlpha3Code!,
                x.MaintenanceType,
                x.VehicleColor,
                x.LastModified,
                x.CreatedDate
            ));
    }
    
    public async ValueTask<bool> CanEditReport(BaseAppUser? r, VehicleReport model)
    {
        if (r is null || r is not VehicleUser requester)
            return false;
        var reqid = requester.Id;

        if (reqid == model.OwnerId)
            return true;

        var manager = provider.GetRequiredService<UserManager<VehicleUser>>();
        return await manager.IsInRoleAsync(requester, VehicleUserRole.AdminReportViewerRole);
    }

    public async ValueTask<bool> CanEditReport(BaseAppUser? r, Snowflake id)
    {
        if (r is null || r is not VehicleUser requester)
            return false;
        var reqid = requester.Id;

        if (await context.Reports.Where(x => x.Id == id).AnyAsync(x => x.OwnerId == reqid)) 
            return true;

        var manager = provider.GetRequiredService<UserManager<VehicleUser>>();
        return await manager.IsInRoleAsync(requester, VehicleUserRole.AdminReportViewerRole);
    }
}
