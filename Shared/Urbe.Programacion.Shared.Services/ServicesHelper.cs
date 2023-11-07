using System.Reflection;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Urbe.Programacion.Shared.Services.Attributes;

namespace Urbe.Programacion.Shared.Services;
public static class ServicesHelper
{
    public static void RegisterDecoratedServices(this IServiceCollection serviceCollection)
    {
        foreach (var (type, attributes) in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Select(x => (Type: x, Attributes: x.GetCustomAttributes<RegisterServiceAttribute>()))
                .Where(x => x.Attributes is not null && x.Attributes.Any()))
        {
            foreach (var attr in attributes)
                serviceCollection.AddScoped(attr.Interface, type);
        }
    }

    public static void RegisterDecoratedOptions(this IServiceCollection services, IConfiguration config)
    {
        foreach (var (type, attributes) in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Select(x => (Type: x, Attributes: x.GetCustomAttributes<RegisterOptionsAttribute>()))
                .Where(x => x.Attributes is not null && x.Attributes.Any()))
        {
            services.AddOptions();
            var configureOptionsType = typeof(IConfigureOptions<>).MakeGenericType(type);
            var configureNamedOptionsType = typeof(ConfigureNamedOptions<>).MakeGenericType(type);
            var optionsConfigurationCallbackType = typeof(Action<>).MakeGenericType(type);
            var configureNamedOptionsConstructor = configureNamedOptionsType.GetConstructor(
                new Type[] { typeof(string), optionsConfigurationCallbackType }
            )!;

            var decoratedOptionsClosureType = typeof(DecoratedOptionsClosure<>).MakeGenericType(type);
            var decoratedOptionsClosureConstructor = decoratedOptionsClosureType.GetConstructor(
                new Type[] { typeof(string), typeof(IConfiguration) }
            )!;

            foreach (var attr in attributes)
            {
                var name = attr.OptionsName ?? Options.DefaultName;
                var section = attr.SectionName ?? type.Name;

                var closure = decoratedOptionsClosureConstructor.Invoke(new object[] { section, config });

                services.AddSingleton(configureOptionsType, configureNamedOptionsConstructor.Invoke(new object[]
                {
                    name,
                    Delegate.CreateDelegate(optionsConfigurationCallbackType, closure, "Bind")
                })!);
            }
        }
    }

    private class DecoratedOptionsClosure<T>
    {
        readonly string bindingName;
        readonly IConfiguration config;

        public DecoratedOptionsClosure(string bindingName, IConfiguration config)
        {
            this.bindingName = bindingName ?? throw new ArgumentNullException(nameof(bindingName));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Bind(T options) => config.GetSection(bindingName).Bind(options);
    }
}
