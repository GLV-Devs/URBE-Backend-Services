using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.DataTransfer.Responses;

public class UserSelfViewModel : IResponseModel<SocialAPIResponseCode>
{
    public required Guid UserId { get; set; }
    public required string Username { get; set; }
    public required UserSettings Settings { get; set; }
    public required string? ProfilePictureUrl { get; set; }
    public string? RealName { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }
    public bool EmailVerified { get; set; }
    public string? Email { get; set; }

    SocialAPIResponseCode IResponseModel<SocialAPIResponseCode>.APIResponseCode => APIResponseCodeEnum.UserSelfView;
}
