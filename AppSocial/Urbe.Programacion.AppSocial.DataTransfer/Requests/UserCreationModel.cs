namespace Urbe.Programacion.AppSocial.DataTransfer.Requests;

public class UserCreationModel
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Pronouns { get; set; }
    public string? RealName { get; set; }
}
