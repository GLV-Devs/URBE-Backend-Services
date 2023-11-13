namespace Urbe.Programacion.AppSocial.DataTransfer;

[Flags]
public enum UserSettings : uint
{
    AllowRealNamePublicly = 1 << 0,

    AllowNonFollowerViews = 1 << 1,
    AllowAnonymousViews = 1 << 2,

    AllowNonFollowerPostViews = 1 << 3,
    AllowAnonymousPostViews = 1 << 4
}