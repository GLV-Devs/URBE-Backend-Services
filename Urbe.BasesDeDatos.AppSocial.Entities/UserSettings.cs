namespace Urbe.BasesDeDatos.AppSocial.Entities;

[Flags]
public enum UserSettings : ulong
{
    AllowRealNamePublicly = 1 << 0,
    
    AllowNonFollowerViews = 1 << 1,
    AllowAnonymousViews = 1 << 2,

    AllowNonFollowerPostViews = 1 << 3,
    AllowAnonymousPostViews = 1 << 4
}