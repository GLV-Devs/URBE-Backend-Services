using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Urbe.Programacion.Shared.Entities.Interfaces;

public interface IConvertibleProperty
{
    public static abstract ValueConverter ValueConverter { get; }
}
