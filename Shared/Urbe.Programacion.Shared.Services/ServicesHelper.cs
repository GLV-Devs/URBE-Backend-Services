using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Urbe.Programacion.Shared.Services.Attributes;

namespace Urbe.Programacion.Shared.Services;
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
