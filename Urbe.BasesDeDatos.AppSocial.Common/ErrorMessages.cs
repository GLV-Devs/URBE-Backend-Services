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
            new ErrorMessageProperty[]
            {
                new(nameof(email), "correo electrónico", email)
            }
        );

    public static ErrorMessage BadUsername(string username)
        => new(
            $"El nombre de usuario no es válido: {username}",
            nameof(BadUsername),
            new ErrorMessageProperty[]
            {
                new(nameof(username), "nombre de usuario", username)
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
                new ErrorMessageProperty[]
                {
                new(nameof(property), "propiedad", property),
                new(nameof(maxCharacters), "caracteres", mc),
                new(nameof(currentCharacters), "caracteres", cc)
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

    public static ErrorMessage EmailAlreadyInUse(string value)
        => new(
            $"El correo electrónico no puede ser '{value}', debido a que ya esta siendo utilizada por otro usuario.",
            nameof(EmailAlreadyInUse),
            new ErrorMessageProperty[]
            {
                new(nameof(value), "valor", value)
            }
        );

    public static ErrorMessage UsernameAlreadyInUse(string value)
        => new(
            $"El nombre de usuario no puede ser '{value}', debido a que ya esta siendo utilizada por otro usuario.",
            nameof(UsernameAlreadyInUse),
            new ErrorMessageProperty[]
            {
                new(nameof(value), "valor", value)
            }
        );

    public static ErrorMessage NotSupported(string property, string action)
        => new(
            $"{action} {property} no es soportado en estos momentos",
            nameof(NotSupported),
            new ErrorMessageProperty[]
            {
                new(nameof(property), "propiedad", property),
                new(nameof(action), "acción", action)
            }
        );
}
