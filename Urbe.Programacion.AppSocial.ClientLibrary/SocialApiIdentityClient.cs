using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public sealed class SocialApiIdentityClient : SocialApiClientModule
{
    internal SocialApiIdentityClient(SocialApiClient client) : base(client, "api/identity") { }

    public ApiResponseTask CreateNew(UserCreationModel request, CancellationToken ct = default)
        => Post<UserSelfViewModel>(null, ct);

    public ApiResponseTask LogIn(UserLoginModel request, CancellationToken ct = default)
        => Put<UserSelfViewModel, UserLoginModel>(null, request, ct);

    public ApiResponseTask LogOut(CancellationToken ct = default)
        => Delete(null, ct);
}
