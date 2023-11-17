using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.DataTransfer.Responses;

public class PostViewModel : IResponseModel<SocialAPIResponseCode>
{
    public required long Id { get; init; }
    public required UserViewModel? Poster { get; init; }
    public required Guid PosterId { get; init; }
    public required string Content { get; init; }
    public required string PosterThenUsername { get; init; }
    public required DateTimeOffset DatePosted { get; init; }
    public required long? InResponseTo { get; init; }
    public required HashSet<long>? Responses { get; init; }

    SocialAPIResponseCode IResponseModel<SocialAPIResponseCode>.APIResponseCode => APIResponseCodeEnum.PostView;
}
