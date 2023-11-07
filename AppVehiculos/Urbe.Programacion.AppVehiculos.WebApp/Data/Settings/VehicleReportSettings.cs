using Urbe.Programacion.Shared.Services.Attributes;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Settings;

[RegisterOptions]
public class VehicleReportSettings
{
    public TimeSpan VehicleReportEditingWindow { get; init; }
}
