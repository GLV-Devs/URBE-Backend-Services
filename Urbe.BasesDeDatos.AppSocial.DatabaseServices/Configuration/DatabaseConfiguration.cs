using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Configuration;

public readonly record struct DatabaseConfiguration(DatabaseType DatabaseType)
{
    public static string FormatConnectionString(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return input.Replace(
                "{appdata}",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Urbe.BasesDeDatos.AppSocial", "data"),
                StringComparison.OrdinalIgnoreCase
            ).Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
    }
}
