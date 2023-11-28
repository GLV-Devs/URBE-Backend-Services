using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public sealed class SocialApiUserClient : SocialApiClientModule
{
    internal SocialApiUserClient(SocialApiClient client) : base(client, "api/user") { }

    public ApiResponseTask GetUsers(CancellationToken ct = default)
        => Get("query", ct);

    public ApiResponseTask GetUsers(string query, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(query);
        return Get($"query?$filter=contains(UserName,'{query}')", ct);
    }

    // query

    public ApiResponseTask UnfollowUser(Guid userId, CancellationToken ct = default)
        => Put($"removefollow/{userId}", ct);

    public ApiResponseTask FollowUser(Guid userId, CancellationToken ct = default)
        => Put($"addfollow/{userId}", ct);

    public ApiResponseTask GetMutuals(Guid userId, CancellationToken ct = default)
        => Get($"mutuals/{userId}", ct);
    // query

    public ApiResponseTask GetFollowed(Guid userId, CancellationToken ct = default)
        => Get($"followed/{userId}", ct);
    // query

    public ApiResponseTask GetFollowers(Guid userId, CancellationToken ct = default)
        => Get($"followers/{userId}", ct);
    // query

    public ApiResponseTask GetUser(string username, CancellationToken ct = default)
        => Get(username, ct);

    public ApiResponseTask GetMyMutuals(CancellationToken ct = default)
        => Get("mutuals", ct);
    // query

    public ApiResponseTask GetMyFollowed(CancellationToken ct = default)
        => Get("followed", ct);
    // query

    public ApiResponseTask GetMyFollowers(CancellationToken ct = default)
        => Get("followers", ct);
    // query

    public ApiResponseTask GetMe(CancellationToken ct = default)
        => Get(null, ct);
    // query

    public ApiResponseTask Update(UserUpdateModel request, CancellationToken ct = default)
        => Put(null, request, ct);
}
