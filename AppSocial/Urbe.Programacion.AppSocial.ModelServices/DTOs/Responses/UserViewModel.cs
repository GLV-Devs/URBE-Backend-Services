using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices.API.Responses;
using Urbe.Programacion.Shared.ModelServices.DTOs;

namespace Urbe.Programacion.AppSocial.ModelServices.DTOs.Responses;

public class UserViewModel : IResponseModel<SocialAPIResponseCode>
{
    public required string Username { get; set; }
    public required Guid UserId { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }
    public string? RealName { get; set; }
    public bool? FollowsRequester { get; set; }
    public string? ProfilePictureUrl { get; set; }

    public static UserViewModel FromHiddenUser(SocialAppUser user)
        => new()
        {
            UserId = user.Id,
            Username = user.UserName!,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Pronouns = user.Pronouns
        };

    public static UserViewModel FromUser(SocialAppUser user)
        => new()
        {
            UserId = user.Id,
            Username = user.UserName!,
            ProfilePictureUrl = user.ProfilePictureUrl,
            RealName = user.Settings.HasFlag(Entities.UserSettings.AllowRealNamePublicly) ? user.RealName : null,
            Pronouns = user.Pronouns
        };

    SocialAPIResponseCode IResponseModel<SocialAPIResponseCode>.APIResponseCode => APIResponseCodeEnum.UserView;
}
