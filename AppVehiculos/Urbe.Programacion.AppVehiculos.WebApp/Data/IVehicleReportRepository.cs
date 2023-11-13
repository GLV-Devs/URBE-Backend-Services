using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Models;
using Urbe.Programacion.Shared.ModelServices;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data;

public interface IVehicleReportRepository : IEntityCRUDRepository<VehicleReport, Snowflake, VehicleReportUpdateModel, VehicleReportUpdateModel>
{
    public ValueTask<bool> CanEditReport(BaseAppUser? user,  VehicleReport model);
    public ValueTask<bool> CanEditReport(BaseAppUser? user, Snowflake id);
}