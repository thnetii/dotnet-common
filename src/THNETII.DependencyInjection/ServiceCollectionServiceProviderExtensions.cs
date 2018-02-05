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
            // Create a default ServiceProvider to use to dependency inject
            // services for an injected Service Provider Factory.
            var defaultServiceProvider = services.BuildServiceProvider();

            /// <summary>
            /// Filter predicate to filter for <see cref="ServiceDescriptor"/>
            /// instances containing <see cref="IServiceProviderFactory{}"/>
            /// as the <see cref="ServiceDescriptor.ServiceType"/>.
            /// </summary>
            bool factoryPredicate(ServiceDescriptor desc)
            {
#if NETSTANDARD1_3
                var t = desc.ServiceType.GetTypeInfo();
#elif NETSTANDARD2_0
                var t = desc.ServiceType;
#endif
                if (t.IsGenericType)
                {
                    var genericDef = t.GetGenericTypeDefinition();
                    return genericDef == typeof(IServiceProviderFactory<>);
                }
                return false;
            }

            // The last ServiceDescriptor is significant.
            var factoryDescriptor = services.LastOrDefault(factoryPredicate);
            if (factoryDescriptor != null)
            {
                // Get ServiceProviderFactory Instance
                var factoryService = defaultServiceProvider
                    .GetService(factoryDescriptor.ServiceType);
                if (factoryService != null)
                {
                    var miGeneric = BuildServiceProviderInfo;
                    // Construct a generic method for the <code>TContainerBuilder</code>
                    // the ServiceProviderFactory uses.
                    var miConstructed = miGeneric.MakeGenericMethod(
                        factoryDescriptor.ServiceType.GenericTypeArguments
                        );
                    // Invoke the factory to construct a ServiceProvider out of
                    // The current service collection.
                    serviceProvider = (IServiceProvider)miConstructed.Invoke(
                        null,
                        new[] { factoryService, services }
                        );
                    // The default service provider might be disposable
                    if (defaultServiceProvider is IDisposable disp)
                        disp.Dispose();
                }
            }
            return serviceProvider ?? defaultServiceProvider;
        }
    }
}
