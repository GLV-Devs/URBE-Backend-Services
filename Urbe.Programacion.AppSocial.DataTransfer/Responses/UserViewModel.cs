using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.DataTransfer.Responses;

public class UserViewModel : IResponseModel<SocialAPIResponseCode>
{
    public required string Username { get; set; }
    public required Guid UserId { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }
    public string? RealName { get; set; }
    public bool? FollowsRequester { get; set; }
    public string? ProfilePictureUrl { get; set; }

    SocialAPIResponseCode IResponseModel<SocialAPIResponseCode>.APIResponseCode => APIResponseCodeEnum.UserView;
}
