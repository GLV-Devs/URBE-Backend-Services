namespace Urbe.Programacion.AppSocial.Common;

public static class ErrorMessages
{
    public static ErrorMessage EmailAlreadyConfirmed()
        => new(
            $"El usuario ya verificó su correo electrónico",
            nameof(EmailAlreadyConfirmed),
            null
        );

    public static ErrorMessage VerificationRequestAlreadyActive()
        => new(
            $"El usuario ya posee una verificación de correo activa, y aun no puede pedir otra",
            nameof(VerificationRequestAlreadyActive),
            null
        );

    public static ErrorMessage ActionDisallowed(string action)
        => new(
            $"La accion '{action}' no está permitida para este usuario",
            nameof(ActionDisallowed),
            new ErrorMessageProperty[]
            {
                new(nameof(action), action)
            }
        );

    public static ErrorMessage LoginRequires(string requirement, string user)
        => new(
            $"Iniciar sesión como el usuario {user} requiere {requirement}",
            nameof(LoginRequires),
            new ErrorMessageProperty[]
            {
                new(nameof(requirement), requirement),
                new(nameof(user), user)
            }
        );

    public static ErrorMessage LoginLockedOut(string user)
        => new(
            $"El usuario {user} se encuentra actualmente bloqueado",
            nameof(LoginLockedOut),
            new ErrorMessageProperty[]
            {
                new(nameof(user), user)
            }
        );

    public static ErrorMessage BadLogin()
        => new(
            "Las credenciales son inválidas",
            nameof(BadLogin),
            null
        );

    public static ErrorMessage UserNotFound(string user)
        => new(
            $"No se encontró el usuario: {user}",
            nameof(UserNotFound),
            new ErrorMessageProperty[]
            {
                new(nameof(user), user)
            }
        );

    public static ErrorMessage InternalError()
        => new(
            $"Ocurrió un error interno en el servidor",
            nameof(InternalError),
            null
        );

    public static ErrorMessage EmptyBody()
        => new(
            "El cuerpo de la petición está vacio",
            nameof(EmptyBody),
            null
        );

    public static ErrorMessage BadEmail(string email)
        => new(
            $"El correo electrónico está malformado: {email}",
            nameof(BadEmail),
            new ErrorMessageProperty[]
            {
                new(nameof(email), email)
            }
        );

    public static ErrorMessage BadUsername(string username)
        => new(
            $"El nombre de usuario no es válido: {username}",
            nameof(BadUsername),
            new ErrorMessageProperty[]
            {
                new(nameof(username), username)
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
                new(nameof(property), property),
                new(nameof(maxCharacters), mc),
                new(nameof(currentCharacters), cc)
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
                new(nameof(value), value)
            }
        );

    public static ErrorMessage UsernameAlreadyInUse(string value)
        => new(
            $"El nombre de usuario no puede ser '{value}', debido a que ya esta siendo utilizada por otro usuario.",
            nameof(UsernameAlreadyInUse),
            new ErrorMessageProperty[]
            {
                new(nameof(value), value)
            }
        );

    public static ErrorMessage NotSupported(string property, string action)
        => new(
            $"{action} {property} no es soportado en estos momentos",
            nameof(NotSupported),
            new ErrorMessageProperty[]
            {
                new(nameof(property), property),
                new(nameof(action), action)
            }
        );
}
