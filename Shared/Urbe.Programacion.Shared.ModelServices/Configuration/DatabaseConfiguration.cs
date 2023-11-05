namespace Urbe.Programacion.Shared.ModelServices.Configuration;

public readonly record struct DatabaseConfiguration(DatabaseType DatabaseType, string SQLServerConnectionString, string SQLiteConnectionString)
{
    public static string FormatConnectionString(string input, string? subfolder = null)
    {
        ArgumentNullException.ThrowIfNull(input);
        return input.Replace(
                "{appdata}",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Urbe.Programacion", subfolder ?? "", "data"),
                StringComparison.OrdinalIgnoreCase
            ).Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
    }
}
