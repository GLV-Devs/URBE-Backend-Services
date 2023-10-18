namespace Urbe.BasesDeDatos.AppSocial.Common;

public static class ErrorMessages
{
    public static ErrorMessage InternalError()
        => new(
            $"Ocurrió un error interno en el servidor",
            nameof(InternalError),
            null
        );

    public static ErrorMessage BadEmail(string email)
        => new(
            $"El correo electrónico está malformado: {email}",
            nameof(BadEmail),
            new Dictionary<string, string>()
            {
                { nameof(email), email }
            }
        );

    public static ErrorMessage BadUsername(string username)
        => new(
            $"El nombre de usuario no es válido: {username}",
            nameof(BadUsername),
            new Dictionary<string, string>()
            {
                { nameof(username), username }
            }
        );

    public static ErrorMessage BadPassword()
        => new(
            "La contraseña ingresada es inválida",
            nameof(BadPassword),
            null
        );

    public static ErrorMessage TooLong(string property, int maxCharacters, int currentCharacters)
    {
        var mc = maxCharacters.ToString();
        var cc = currentCharacters.ToString();
        return new(
                $"La propiedad {property} tiene demasiados caracteres ({cc}). Esta propiedad tiene un maximo de {mc} caracteres",
                nameof(TooLong),
                new Dictionary<string, string>()
                {
                { nameof(property), property },
                { nameof(maxCharacters), mc },
                { nameof(currentCharacters), cc }
                }
            );
    }

    public static ErrorMessage NoPermission()
        => new(
            "El usuario no tiene permisos para realizar ésta acción",
            nameof(NoPermission),
            null
        );

    public static ErrorMessage NoPostContent()
        => new(
            "La petición no cuenta con contenido en el post",
            nameof(NoPostContent),
            null
        );

    public static ErrorMessage AlreadyInUse(string property, string value)
        => new(
            $"La propiedad {property} no puede contener el valor '{value}', debido a que ya esta siendo utilizada.",
            nameof(AlreadyInUse),
            new Dictionary<string, string>()
            {
                { nameof(property), property },
                { nameof(value), value }
            }
        );

    public static ErrorMessage AlreadyInUseByOtherUser(string property, string value)
        => new(
            $"La propiedad {property} no puede contener el valor '{value}', debido a que ya esta siendo utilizada por otro usuario.",
            nameof(AlreadyInUseByOtherUser),
            new Dictionary<string, string>()
            {
                { nameof(property), property },
                { nameof(value), value }
            }
        );

    public static ErrorMessage NotSupported(string property, string action)
        => new(
            $"{action} {property} no es soportado en estos momentos",
            nameof(NotSupported),
            new Dictionary<string, string>()
            {
                { nameof(property), property },
                { nameof(action), action }
            }
        );
}
