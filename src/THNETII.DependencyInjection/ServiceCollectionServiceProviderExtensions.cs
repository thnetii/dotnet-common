using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace THNETII.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for the <see cref="IServiceCollection"/>
    /// interface to build <see cref="IServiceProvider"/> instances.
    /// </summary>
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
            .GetMethod(nameof(BuildServiceProvider),
                BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Creates an <see cref="IServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>.
        /// <para> If possible, a registered <see cref="IServiceProviderFactory{TContainerBuilder}"/> will be used to create the <see cref="IServiceProvider"/> instance.</para>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
        /// <returns>A new <see cref="IServiceProvider"/> instance.</returns>
        /// <remarks>
        /// If <paramref name="services"/> contains a <see cref="ServiceDescriptor"/> with the <see cref="IServiceProviderFactory{TContainerBuilder}"/> type,
        /// this extension method will create a new service provider factory and use it to create the serivice provider.
        /// <para>
        /// If <paramref name="services"/> does not contain any <see cref="IServiceProviderFactory{TContainerBuilder}"/> service descriptors,
        /// a default <see cref="IServiceProvider"/> is created by calling <see cref="ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(IServiceCollection)"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
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
                }
            }
            return serviceProvider ?? defaultServiceProvider;
        }
    }
}
