using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;

using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public sealed class SocialApiPostClient : SocialApiCRDModule<Snowflake, PostCreationModel>
{
    internal SocialApiPostClient(SocialApiClient client) : base(client, "api/post") { }

    public ApiResponseTask GetResponses(Snowflake postId, CancellationToken ct = default)
        => Get($"responses/{postId}", ct);
    // query

    public ApiResponseTask GetMyPosts(CancellationToken ct = default)
        => Get("me", ct);
    // query

    public ApiResponseTask GetFeed(CancellationToken ct = default)
        => Get("feed", ct);

    public ApiResponseTask GetUserPosts(Guid userId, CancellationToken ct = default)
        => Get($"from/{userId}", ct);
    // query

    public ApiResponseTask LikePost(Snowflake postId, CancellationToken ct = default)
        => Put($"like/{postId}", ct);

    public ApiResponseTask UnlikePost(Snowflake postId, CancellationToken ct = default)
        => Put($"unlike/{postId}", ct);
}
