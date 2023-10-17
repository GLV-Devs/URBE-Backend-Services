using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Configuration;

public readonly record struct DatabaseConfiguration(DatabaseType DatabaseType)
{
    public static string ReplaceConnectionStringWildCards(string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return input.Replace(
                "{appdata}",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Urbe.BasesDeDatos.AppSocia", "data"),
                StringComparison.OrdinalIgnoreCase
            );
    }
}
