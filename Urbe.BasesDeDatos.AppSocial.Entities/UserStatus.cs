namespace Urbe.BasesDeDatos.AppSocial.Entities;

[Flags]
public enum UserStatus : ulong
{
    UserVerified = 1 << 0,
    EmailVerified = 1 << 1
}