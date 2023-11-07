namespace Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

[Flags]
public enum VehicleMaintenanceType : uint
{
    Corrective = 1 << 0,
    Preventive = 1 << 1
}

public static class VehicleMaintenanceTypeExtensions
{
    public static bool IsValid(this VehicleMaintenanceType vehicleMaintenanceType)
        => (((uint)vehicleMaintenanceType) & ~3u) == 0;
}