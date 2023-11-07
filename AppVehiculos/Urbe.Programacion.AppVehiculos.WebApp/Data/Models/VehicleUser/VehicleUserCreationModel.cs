namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Models.VehicleUser;

public class VehicleUserCreationModel
{
    public string? Username { get; set; }
    public string? RealName { get; set; }
    public string? Email { get; set; }
    public string? ConfirmEmail { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

    public VehicleUserCreationModel(string? username, string? realName, string? email, string? confirmEmail, string? password, string? confirmPassword)
    {
        Username = username;
        RealName = realName;
        Email = email;
        ConfirmEmail = confirmEmail;
        Password = password;
        ConfirmPassword = confirmPassword;
    }
}
