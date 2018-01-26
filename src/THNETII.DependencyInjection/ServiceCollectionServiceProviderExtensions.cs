using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace THNETII.DependencyInjection
{
    public static class ServiceCollectionServiceProviderExtensions
    {
        private static IServiceProvider BuildServiceProvider<TContainerBuilder>(
            IServiceProviderFactory<TContainerBuilder> factory,
            IServiceCollection services)
        {
            var builder = factory.CreateBuilder(services);
            return factory.CreateServiceProvider(builder);
        }

        private static MethodInfo BuildServiceProviderInfo =
            typeof(ServiceCollectionServiceProviderExtensions)
            .GetMethod(
                nameof(BuildServiceProvider),
                BindingFlags.NonPublic | BindingFlags.Static
                );

        public static IServiceProvider Build(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            IServiceProvider serviceProvider = null;
            var defaultServiceProvider = services.BuildServiceProvider();
            bool factoryPredicate(ServiceDescriptor desc)
            {
#if NETSTANDARD1_3
                var t = desc.ServiceType.GetTypeInfo();
#elif NETSTANDARD2_0
                var t = desc.ServiceType;
#endif
                if (t.IsGenericType)
                {
                    var def = t.GetGenericTypeDefinition();
                    return def == typeof(IServiceProviderFactory<>);
                }
                return false;
            }
            var factoryDescriptor = services.LastOrDefault(factoryPredicate);
            if (factoryDescriptor != null)
            {
                var factoryService = defaultServiceProvider.
                    GetService(factoryDescriptor.ServiceType);
                if (factoryService != null)
                {
                    var miGeneric = BuildServiceProviderInfo;
                    var miConstructed = miGeneric.MakeGenericMethod(
                        factoryDescriptor.ServiceType.GenericTypeArguments
                        );
                    serviceProvider = (IServiceProvider)miConstructed.Invoke(
                        null,
                        new[] { factoryService, services }
                        );
                    if (defaultServiceProvider is IDisposable disp)
                        disp.Dispose();
                }
            }
            return serviceProvider ?? defaultServiceProvider;
        }
    }
}
