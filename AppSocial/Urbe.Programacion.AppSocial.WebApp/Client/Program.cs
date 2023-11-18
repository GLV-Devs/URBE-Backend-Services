using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Urbe.Programacion.AppSocial.ClientLibrary;
using Urbe.Programacion.AppSocial.WebApp.Client;

namespace Urbe.Programacion.AppSocial.WebApp.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddSingleton<CookieHandler>();
        builder.Services.AddScoped(
            sp => new HttpClient(sp.GetRequiredService<CookieHandler>()) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
        );

        builder.Services.AddScoped<SocialApiClient>();
        builder.Services.AddSingleton<AppState>();

        await builder.Build().RunAsync();
    }
}
