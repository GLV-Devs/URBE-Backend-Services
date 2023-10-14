namespace Urbe.BasesDeDatos.AppSocial.Common;

public static class ErrorMessages
{
    public static ErrorMessage BadEmail(string email)
        => new($"El correo electrónico está malformado: {email}");
}
