using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.BasesDeDatos.AppSocial.Common;
public static class Helper
{
    public static bool IsUpdating(string original, [NotNullWhen(true)] string? update)
        => update is not null && string.Equals(original, update, StringComparison.OrdinalIgnoreCase) is false;
}
