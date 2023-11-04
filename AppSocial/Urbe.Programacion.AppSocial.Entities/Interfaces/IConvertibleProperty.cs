using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Urbe.Programacion.AppSocial.Entities.Interfaces;

public interface IConvertibleProperty
{
    public static abstract ValueConverter ValueConverter { get; }
}
