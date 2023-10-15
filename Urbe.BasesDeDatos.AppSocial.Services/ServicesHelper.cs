using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Urbe.BasesDeDatos.AppSocial.Services.Attributes;

namespace Urbe.BasesDeDatos.AppSocial.Services;
public static class ServicesHelper
{
    public static void RegisterDatabaseServices(this IServiceCollection serviceCollection)
    {
        foreach (var (type, attr) in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Select(x => (Type: x, Attribute: x.GetCustomAttribute<RegisterServiceAttribute>()))
                .Where(x => x.Attribute is not null))
        {
            Debug.Assert(attr is not null);
            serviceCollection.AddScoped(attr.Interface, type);
        }
    }
}
