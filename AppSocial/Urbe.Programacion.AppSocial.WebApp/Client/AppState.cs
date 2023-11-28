using Majorsoft.Blazor.Extensions.BrowserStorage;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client;

public class AppState
{
    private const string TokenKey = "AppSocial.BearerToken";
    private readonly ILocalStorageService LocalStorage;

    public AppState(ILocalStorageService localStorageService)
    {
        LocalStorage = localStorageService;
    }

    public async Task<string?> GetToken() 
        => await LocalStorage.ContainKeyAsync(TokenKey) ? await LocalStorage.GetItemAsStringAsync(TokenKey) : null;

    public async Task SetToken(string? token)
        => await LocalStorage.SetItemAsync(TokenKey, token);

    public UserSelfViewModel? LoggedInUser { get; set; }
}
