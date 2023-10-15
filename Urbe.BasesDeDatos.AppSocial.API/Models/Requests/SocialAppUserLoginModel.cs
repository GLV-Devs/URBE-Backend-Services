namespace Urbe.BasesDeDatos.AppSocial.API.Models.Requests;

public class SocialAppUserLoginModel
{
    public string? UserNameOrEmail { get; set; }
    public string? Password { get; set; }
}
