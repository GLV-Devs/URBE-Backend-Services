using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs.Responses;

public class UserViewModel
{
    public required string Username { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }
    public string? RealName { get; set; }
    public bool? FollowsRequester { get; set; }
    public required string? ProfilePictureUrl { get; set; }

    public static UserViewModel FromHiddenUser(SocialAppUser user)
        => new()
        {
            Username = user.UserName!,
            ProfilePictureUrl = null
        };

    public static UserViewModel FromUser(SocialAppUser user)
        => new()
        {
            Username = user.UserName!,
            ProfilePictureUrl = user.ProfilePictureUrl,
            RealName = user.Settings.HasFlag(Entities.UserSettings.AllowRealNamePublicly) ? user.RealName : null
        };
}
