namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Identity;

public class LogInRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool StayLoggedIn { get; set; }

    public LogInRequest(string userName, string password, bool stayLoggedIn)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        StayLoggedIn = stayLoggedIn;
    }
}
