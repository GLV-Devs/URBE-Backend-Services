namespace Urbe.Programacion.Shared.Services.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class RegisterOptionsAttribute : Attribute 
{
    public string? SectionName { get; init; }
    public string? OptionsName { get; init; }
}
