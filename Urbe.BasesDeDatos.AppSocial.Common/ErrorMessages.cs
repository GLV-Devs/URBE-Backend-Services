namespace Urbe.BasesDeDatos.AppSocial.Common;

public static class ErrorMessages
{
    public static ErrorMessage BadEmail(string email)
        => new($"El correo electrónico está malformado: {email}");

    public static ErrorMessage TooLong(string property, int maxCharacters, int currentCharacters)
        => new($"La propiedad {property} tiene demasiados caracteres ({currentCharacters}). Esta propiedad tiene un maximo de {maxCharacters} caracteres");
}
