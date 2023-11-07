namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleUser;

public class VehicleUserView
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? RealName { get; set; }

    public VehicleUserView(string? username, string? email, string? realName)
    {
        Username = username;
        Email = email;
        RealName = realName;
    }
}
