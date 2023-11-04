using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Urbe.Programacion.AppSocial.Services.Attributes;

namespace Urbe.Programacion.AppSocial.Services;
public static class ServicesHelper
{
    public static void RegisterDecoratedServices(this IServiceCollection serviceCollection)
    {
        foreach (var (type, attributes) in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Select(x => (Type: x, Attributes: x.GetCustomAttributes<RegisterServiceAttribute>())))
        {
            foreach (var attr in attributes)
                serviceCollection.AddScoped(attr.Interface, type);
        }
    }
}
