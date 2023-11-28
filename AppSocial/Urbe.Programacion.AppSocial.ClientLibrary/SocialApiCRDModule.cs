using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public class SocialApiCRDModule<TKey, TCreationModel> : SocialApiClientModule
{
    internal SocialApiCRDModule(SocialApiClient client, string controller) : base(client, controller) { }

    public ApiResponseTask Create(TCreationModel creationModel, CancellationToken ct = default)
        => Post(null, creationModel, ct);

    public ApiResponseTask Query(CancellationToken ct = default)
        => Get(null, ct);

    public ApiResponseTask View(TKey key, CancellationToken ct = default)
        => Get($"{key}", ct);

    public ApiResponseTask Delete(TKey key, CancellationToken ct = default)
        => Delete($"{key}", ct);
}
