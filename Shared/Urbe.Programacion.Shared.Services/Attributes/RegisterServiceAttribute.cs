using Microsoft.Extensions.DependencyInjection;

namespace Urbe.Programacion.Shared.Services.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class RegisterServiceAttribute : Attribute
{
    public Type? Interface { get; init; }
    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Scoped;

    public RegisterServiceAttribute(Type? type = null)
    {
        Interface = type;
    }
}