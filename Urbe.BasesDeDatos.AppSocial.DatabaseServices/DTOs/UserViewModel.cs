using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;

public class UserViewModel
{
    public required string Username { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }
    public string? RealName { get; set; }
    public required string? ProfilePictureUrl { get; set; }

    public static UserViewModel FromUser(SocialAppUser user)
        => new()
        {
            Username = user.UserName!,
            ProfilePictureUrl = user.ProfilePictureUrl,
            RealName = user.Settings.HasFlag(Entities.UserSettings.AllowRealNamePublicly) ? user.RealName : null
        };
}
