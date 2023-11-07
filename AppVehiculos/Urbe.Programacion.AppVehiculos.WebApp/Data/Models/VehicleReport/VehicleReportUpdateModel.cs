using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;

public class VehicleReportUpdateModel
{
    public string? VehicleModel { get; set; }
    public string? VehicleMake { get; set; }
    public string? LicensePlate { get; set; }
    public uint? VehicleColor { get; set; }
    public string? VehicleCountryAlpha3Code { get; set; }
    public VehicleMaintenanceType? MaintenanceType { get; set; }
}
