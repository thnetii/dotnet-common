using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;

using THNETII.Common;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// A singleton dependency injection service that manages nested service
    /// provider instances under a root service provider.
    /// <para>
    /// This class should be used in conjunction with the <see cref="NestedScopedServicesManager{TFamily, TKey}"/>
    /// to properly enforce nested scoping boundaries.
    /// </para>
    /// </summary>
    /// <typeparam name="TFamily">The type for the family of the nested containers this manager is managing.</typeparam>
    /// <typeparam name="TKey">The type of the key for the nested service container the instance is managing.</typeparam>
    public class NestedSingletonServicesManager<TFamily, TKey>
    {
        private readonly IDictionary<TKey, IServiceCollection> nestedServices;
        private readonly IDictionary<TKey, WeakReference<IServiceProvider>> nestedServiceProviders;
        private readonly IServiceProvider rootServiceProvider;

        /// <summary>
        /// Creates a new nested singleton manager instance using the specified
        /// parent service container and the nested service collections.
        /// </summary>
        /// <param name="rootServiceProvider">The parent container that is shared by the nested containers that are to be managed.</param>
        /// <param name="nestedServices">The nested service collections that have been registered for the specified root service provider. Must not be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nestedServices"/> is <see langword="null"/>.</exception>
        public NestedSingletonServicesManager(IServiceProvider rootServiceProvider,
            IEnumerable<INestedServiceCollection<TFamily, TKey>> nestedServices)
        {
            this.rootServiceProvider = rootServiceProvider;
            IEqualityComparer<TKey> keyComparer = NestingKeyComparer<TKey>.
                GetNestingKeyComparer(rootServiceProvider);
            this.nestedServices = nestedServices
                .ThrowIfNull(nameof(nestedServices))
                .ToDictionary(
                    nsc => nsc.Key,
                    nsc => nsc as IServiceCollection,
                    keyComparer
                    );
            nestedServiceProviders = new Dictionary<TKey, WeakReference<IServiceProvider>>(
                keyComparer);
        }

        /// <summary>
        /// Retrieves or creates a service provider using the service container
        /// with the specified key.
        /// </summary>
        /// <param name="key">The key of the service container to use.</param>
        /// <returns>
        /// An <see cref="IServiceProvider"/> instance resolving the services
        /// registered to the container with the specified key.
        /// </returns>
        public IServiceProvider GetServiceProvider(TKey key)
        {
            lock (nestedServiceProviders)
            {
                if (!nestedServiceProviders.TryGetValue(key, out var spRef) ||
                    !spRef.TryGetTarget(out var serviceProvider))
                {
                    serviceProvider = CreateNestedServiceProvider(key);
                    nestedServiceProviders[key] =
                        new WeakReference<IServiceProvider>(serviceProvider);
                }
                return serviceProvider;
            }
        }

        private IServiceProvider CreateNestedServiceProvider(TKey key)
        {
            var services = GetNestedServices(key);
            services.AddSingleton(
                new RootServiceProviderAccessor(rootServiceProvider));
            return services.Build();
        }

        private IServiceCollection GetNestedServices(TKey key)
        {
            lock (nestedServices)
            {
                if (!nestedServices.TryGetValue(key, out var services))
                {
                    services = new ServiceCollection();
                    nestedServices[key] = services;
                }
                return services;
            }
        }
    }
}
