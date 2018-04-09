using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using THNETII.Common;

namespace THNETII.DependencyInjection.Nesting
{
    public class NestedSingletonServicesManager<TFamily, TKey>
    {
        private readonly IDictionary<TKey, IServiceCollection>
             nestedServices;
        private readonly IDictionary<TKey, WeakReference<IServiceProvider>> nestedServiceProviders;
        private readonly IServiceProvider rootServiceProvider;

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
