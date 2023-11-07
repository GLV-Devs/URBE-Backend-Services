using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.Shared.Entities;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleReport;

public class VehicleReportView
{
    public Snowflake Id { get; set; }
    public Guid OwnerId { get; set; }
    public string VehicleModel { get; set; }
    public string VehicleMake { get; set; }
    public string LicensePlate { get; set; }
    public string VehicleCountryAlpha3Code { get; set; }
    public string OwnerName { get; set; }
    public VehicleMaintenanceType MaintenanceType { get; set; }
    public uint? VehicleColor { get; set; }
    public DateTimeOffset LastEdited { get; set; }
    public DateTimeOffset Created { get; set; }

    public VehicleReportView(
        Snowflake id,
        Guid ownerId,
        string vehicleModel,
        string licensePlate,
        string vehicleCountryAlpha3Code,
        VehicleMaintenanceType maintenanceType,
        uint? vehicleColor,
        DateTimeOffset lastEdited,
        DateTimeOffset created,
        string ownerName,
        string vehicleMake
        )
    {
        Id = id;
        OwnerId = ownerId;
        VehicleMake = vehicleMake ?? throw new ArgumentNullException(nameof(vehicleMake));
        VehicleModel = vehicleModel ?? throw new ArgumentNullException(nameof(vehicleModel));
        LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
        VehicleCountryAlpha3Code = vehicleCountryAlpha3Code ?? throw new ArgumentNullException(nameof(vehicleCountryAlpha3Code));
        MaintenanceType = maintenanceType;
        VehicleColor = vehicleColor;
        LastEdited = lastEdited;
        Created = created;
        OwnerName = ownerName ?? throw new ArgumentNullException(nameof(ownerName));
    }
}
