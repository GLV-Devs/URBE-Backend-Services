using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.Programacion.AppSocial.ModelServices.Configuration;

public readonly record struct DatabaseConfiguration(DatabaseType DatabaseType, string SQLServerConnectionString, string SQLiteConnectionString)
{
    public static string FormatConnectionString(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return input.Replace(
                "{appdata}",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Urbe.Programacion.AppSocial", "data"),
                StringComparison.OrdinalIgnoreCase
            ).Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
    }
}
