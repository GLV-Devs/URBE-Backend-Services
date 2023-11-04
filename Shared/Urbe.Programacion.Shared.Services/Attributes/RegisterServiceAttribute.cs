namespace Urbe.Programacion.Shared.Services.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class RegisterServiceAttribute : Attribute
{
    public Type Interface { get; }

    public RegisterServiceAttribute(Type type)
    {
        Interface = type;
    }
}