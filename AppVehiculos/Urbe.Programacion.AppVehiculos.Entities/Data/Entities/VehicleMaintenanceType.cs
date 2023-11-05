namespace Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

[Flags]
public enum VehicleMaintenanceType
{
    Corrective = 1 << 0,
    Preventive = 1 << 1
}
