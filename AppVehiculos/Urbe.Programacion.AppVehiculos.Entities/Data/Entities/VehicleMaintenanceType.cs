using System.Text;

namespace Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

[Flags]
public enum VehicleMaintenanceType : uint
{
    Corrective = 1 << 0,
    Preventive = 1 << 1
}

public static class VehicleMaintenanceTypeExtensions
{
    [ThreadStatic]
    private static StringBuilder? Sb;

    public static bool IsValid(this VehicleMaintenanceType vehicleMaintenanceType)
        => (((uint)vehicleMaintenanceType) & ~3u) == 0;

    public static string ToStringESP(this VehicleMaintenanceType vehicleMaintenanceType)
    {
        var sb = Sb ??= new StringBuilder(nameof(VehicleMaintenanceType.Corrective).Length + nameof(VehicleMaintenanceType.Preventive).Length + 3);
        Sb.Clear();

        if (vehicleMaintenanceType.HasFlag(VehicleMaintenanceType.Corrective))
        {
            sb.Append("Correctivo");
            if (vehicleMaintenanceType.HasFlag(VehicleMaintenanceType.Preventive))
                sb.Append(" y Preventivo");
        }
        else if (vehicleMaintenanceType.HasFlag(VehicleMaintenanceType.Preventive))
            sb.Append("Preventivo");
        else
            sb.Append("Desconocido");

        return sb.ToString();
    }
}