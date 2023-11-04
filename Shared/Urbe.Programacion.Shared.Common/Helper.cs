using System.Diagnostics.CodeAnalysis;

namespace Urbe.Programacion.Shared.Common;
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
