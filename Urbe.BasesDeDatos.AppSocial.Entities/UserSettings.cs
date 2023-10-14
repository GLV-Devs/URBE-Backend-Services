namespace Urbe.BasesDeDatos.AppSocial.Entities;

[Flags]
public enum UserSettings : ulong
{
    AllowRealNamePublicly = 1 << 0,
    AllowPublicViews = 1 << 1
}