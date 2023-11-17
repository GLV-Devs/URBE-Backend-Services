using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public sealed class SocialApiVerificationClient : SocialApiClientModule
{
    internal SocialApiVerificationClient(SocialApiClient client) : base(client, "api/verification") { }

    public ApiResponseTask RequestVerification(CancellationToken ct = default)
        => Get(null, ct);

    public ApiResponseTask Verify(string token, CancellationToken ct = default)
        => Get(token, ct);
}