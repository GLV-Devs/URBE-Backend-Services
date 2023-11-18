using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Urbe.Programacion.Shared.Common;

public static partial class ErrorMessages
{
    public static ErrorMessage PropertiesNotEqual(string property, string otherProperty)
        => new(
            $"{property} no es igual a {otherProperty}",
            nameof(PropertiesNotEqual),
            new ErrorMessageProperty[] 
            { 
                new(nameof(property), property),
                new(nameof(otherProperty), otherProperty)
            }
        );

    public static ErrorMessage InvalidServerHttpCode(HttpStatusCode code)
        => new(
            $"El servidor envió un código de respuesta inesperado: {(int)code} {Enum.GetName(code)}",
            nameof(InvalidServerHttpCode),
            new ErrorMessageProperty[] { new("code", ((int)code).ToString()) }
        );

    public static ErrorMessage UnexpectedServerResponse(int code, string? name = null)
        => new(
            $"El servidor envió una respuesta inesperada: {code} {name}",
            nameof(UnexpectedServerResponse),
            new ErrorMessageProperty[] { new("code", ((int)code).ToString()) }
        );

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

    public static ErrorMessage ConfirmationNotSame(string property)
        => new(
            $"El campo de confirmación para la propiedad '{property}' no coincide con la misma",
            nameof(ConfirmationNotSame),
            new ErrorMessageProperty[]
            {
                new(nameof(property), property)
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

    public static ErrorMessage InternalError(string? message = null)
        => new(
            string.IsNullOrWhiteSpace(message) ? "Ocurrió un error interno en el servidor" : $"Ocurrió un error interno en el servidor: {message}",
            nameof(InternalError),
            string.IsNullOrWhiteSpace(message)
            ? null
            : new ErrorMessageProperty[] { new(nameof(message), message) }
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

    public static ErrorMessage InvalidProperty(string property)
        => new(
            $"La propiedad es inválida: {property}",
            nameof(InvalidProperty),
            new ErrorMessageProperty[]
            {
                new(nameof(property), property)
            }
        );

    public static ErrorMessage EmptyProperty(string? property = null)
        => new(
            string.IsNullOrWhiteSpace(property) ? "La propiedad no puede permanecer vacía" : $"La propiedad no puede permanecer vacía: {property}",
            nameof(EmptyProperty),
            string.IsNullOrWhiteSpace(property) ? null : new ErrorMessageProperty[] { new(nameof(property), property) }
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

    public static ErrorMessage TimedOut(string action)
        => new(
            $"La acción expiró y ya no está disponible: {action}",
            nameof(TimedOut),
            new ErrorMessageProperty[]
            {
                new(nameof(action), action)
            }
        );

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

    public static ErrorMessage PasswordRequiredUniqueChars(int uniqueCharCount = 4)
        => new(
            $"La contraseña debe contener al menos {uniqueCharCount} caracteres únicos",
            nameof(PasswordTooShort),
            new ErrorMessageProperty[]
            {
                new(nameof(uniqueCharCount), uniqueCharCount.ToString())
            }
        );

    public static ErrorMessage PasswordTooShort(int minimumLength = 6)
        => new(
            $"La contraseña debe contener al menos {minimumLength} caracteres",
            nameof(PasswordTooShort),
            new ErrorMessageProperty[]
            {
                new(nameof(minimumLength), minimumLength.ToString())
            }
        );

    public static ErrorMessage PasswordRequiresLower()
        => new(
            $"La contraseña debe contener al menos un caracter en minúscula",
            nameof(PasswordRequiresLower),
            null
        );

    public static ErrorMessage PasswordRequiresNonAlphanumeric()
        => new(
            $"La contraseña debe contener al menos un caracter que no sea alfanumerico",
            nameof(PasswordRequiresNonAlphanumeric),
            null
        );

    public static ErrorMessage PasswordRequiresUpper()
        => new(
            $"La contraseña debe contener al menos un caracter en mayúscula",
            nameof(PasswordRequiresUpper),
            null
        );

    public static string EncodeErrorMessage(string key, params object[]? arguments)
        => $"{key}:{(arguments is not null ? string.Join(',', arguments.Select(EncodeArgument)) : null)}";

    private static string? EncodeArgument(object argument)
    {
        var reg = EncodeCleanupRegex();
        if (argument is string str)
            return (string?)$"\"{reg.Replace(str, "")}\"";
        else 
        {
            var strarg = argument.ToString();
            return strarg is not null ? reg.Replace(strarg, "") : null;
        }
    }

    //public static (string Key, object[]? Arguments) DecodeErrorMessage(string encoded)
    //{
    //    var split = encoded.Split(':');
    //    return (split[0], )
    //}

    public static ErrorMessage TryBindError(string key, string? description, params object[]? arguments)
    {
        var method = typeof(ErrorMessages).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name.Equals(key));
        if (method is not null)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != arguments?.Length)
            {
                if (parameters.Length > (arguments?.Length ?? 0))
                {
                    var newargs = new object[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                        if (i < (arguments?.Length ?? 0))
                            newargs[i] = arguments![i];
                        else if (parameters[i].HasDefaultValue)
                            newargs[i] = parameters[i].DefaultValue!;
                        else
                            return new ErrorMessage(description, key, null);

                    return (ErrorMessage)method.Invoke(null, newargs)!;
                }
            }

            return (ErrorMessage)method.Invoke(null, arguments)!;
        }

        return new ErrorMessage(description, key, null);
    }

    [GeneratedRegex(@"["":]")]
    private static partial Regex EncodeCleanupRegex();
}
