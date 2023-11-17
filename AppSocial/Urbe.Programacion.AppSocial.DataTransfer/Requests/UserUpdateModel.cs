using System.Reflection.PortableExecutable;

namespace Urbe.Programacion.AppSocial.DataTransfer.Requests;

public class UserUpdateModel
{
    public string? RealName { get; set; }
    public string? Username { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }
    public string? Email { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public UserSettings? UserSettings { get; set; }
}
