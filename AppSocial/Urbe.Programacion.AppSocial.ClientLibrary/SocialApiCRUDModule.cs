using ApiResponseTask = System.Threading.Tasks.Task<Urbe.Programacion.AppSocial.ClientLibrary.SocialApiRequestResponse>;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public class SocialApiCRUDModule<TKey, TUpdateModel, TCreationModel> : SocialApiCRDModule<TKey, TCreationModel>
{
    internal SocialApiCRUDModule(SocialApiClient client, string controller) : base(client, controller) { }

    public ApiResponseTask Update(TKey key, TUpdateModel update, CancellationToken ct = default)
        => Put(null, update, ct);
}
