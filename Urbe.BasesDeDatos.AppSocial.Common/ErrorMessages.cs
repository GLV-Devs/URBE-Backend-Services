namespace Urbe.BasesDeDatos.AppSocial.Common;

public static class ErrorMessages
{
    public static ErrorMessage BadEmail(string email)
        => new($"El correo electrónico está malformado: {email}");

    public static ErrorMessage BadUsername(string username)
        => new($"El nombre de usuario no es válido: {username}");

    public static ErrorMessage BadPassword()
        => new("La contraseña ingresada es inválida");

    public static ErrorMessage TooLong(string property, int maxCharacters, int currentCharacters)
        => new($"La propiedad {property} tiene demasiados caracteres ({currentCharacters}). Esta propiedad tiene un maximo de {maxCharacters} caracteres");

    public static ErrorMessage NoPermission()
        => new("El usuario no tiene permisos para realizar ésta acción");

    public static ErrorMessage NoPostContent()
        => new("La petición no cuenta con contenido en el post");

    public static ErrorMessage AlreadyInUse(string property, string value)
        => new($"La propiedad {property} no puede contener el valor '{value}', debido a que ya esta siendo utilizada.");

    public static ErrorMessage AlreadyInUseByOtherUser(string property, string value)
        => new($"La propiedad {property} no puede contener el valor '{value}', debido a que ya esta siendo utilizada por otro usuario.");

    public static ErrorMessage NotSupported(string property, string action)
        => new($"{action} {property} no es soportado en estos momentos");
}
