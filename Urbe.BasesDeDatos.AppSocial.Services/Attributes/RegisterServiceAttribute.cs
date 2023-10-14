using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.BasesDeDatos.AppSocial.Services.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class RegisterServiceAttribute : Attribute
{
    public Type Interface { get; }

    public RegisterServiceAttribute(Type type)
    {
        Interface = type;
    }
}