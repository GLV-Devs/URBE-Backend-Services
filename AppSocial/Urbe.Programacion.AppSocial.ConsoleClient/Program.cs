using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Urbe.Programacion.AppSocial.ConsoleClient;

public static class Program
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public static ClientConfig Config { get; private set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private static async Task Main(string[] args)
    {
        if (File.Exists("ClientConfig.json"))
        {
            using var file = File.OpenRead("ClientConfig.json");
            Config = (await JsonSerializer.DeserializeAsync<ClientConfig>(file))!;
            Config.Validate();
        }
        else
        {
            using var file = File.OpenWrite("ClientConfig.json");
            await JsonSerializer.SerializeAsync(file, new ClientConfig("Write the API's URL here"));
            throw new FileNotFoundException("Could not find the configurations file. Created a new one, fill it and try again.");
        }

        var csd = new ConsoleScreenDriver();
        //await csd.Run(new LoginScreen());
    }
}
