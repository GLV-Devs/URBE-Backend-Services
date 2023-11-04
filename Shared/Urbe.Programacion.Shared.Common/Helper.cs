using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Urbe.Programacion.AppSocial.Common;
public static class Helper
{
    public static bool IsUpdating(string? original, [NotNullWhen(true)] string? update, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        => update is not null && string.Equals(original, update, comparison) is false;

    public static bool IsTooLong(ref ErrorList errors, string update, int maxlength, string property)
    {
        if (update.Length > maxlength)
        {
            errors.RecommendedCode = System.Net.HttpStatusCode.BadRequest;
            errors.AddError(ErrorMessages.TooLong(property, maxlength, update.Length));
            return true;
        }

        return false;
    }
}
