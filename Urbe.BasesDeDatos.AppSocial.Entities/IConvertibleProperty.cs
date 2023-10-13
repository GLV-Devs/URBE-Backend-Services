using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public interface IConvertibleProperty
{
    public static abstract ValueConverter ValueConverter { get; }
}
