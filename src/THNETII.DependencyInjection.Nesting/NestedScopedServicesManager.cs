using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using THNETII.Common;

namespace THNETII.DependencyInjection.Nesting
{
    public class NestedScopedServicesManager<TFamily, TKey>
        : INestedServicesContainer<TFamily, TKey>, IDisposable
    {
        private readonly NestedSingletonServicesManager<TFamily, TKey> singletonManager;
        private readonly IDictionary<TKey, WeakReference<IServiceScope>> serviceScopes;

        public NestedScopedServicesManager(
            IServiceProvider rootServiceProvider,
            NestedSingletonServicesManager<TFamily, TKey> singletonManager)
        {
            var keyComparer = NestingKeyComparer<TKey>.
                GetNestingKeyComparer(rootServiceProvider);
            serviceScopes = new Dictionary<TKey, WeakReference<IServiceScope>>(keyComparer);
            this.singletonManager = singletonManager.ThrowIfNull(nameof(singletonManager));
        }

        public virtual void Dispose()
        {
            lock (serviceScopes)
            {
                foreach (var weakRef in serviceScopes.Values)
                {
                    if (weakRef.TryGetTarget(out var scope))
                        (scope as IDisposable).Dispose();
                }
                serviceScopes.Clear();
            }
        }

        public virtual IServiceProvider GetServiceProvider(TKey key)
        {
            var serviceProvider = singletonManager.GetServiceProvider(key);
            lock (serviceScopes)
            {
                if (!serviceScopes.TryGetValue(key, out var scopeRef) ||
                    !scopeRef.TryGetTarget(out var serviceScope))
                {
                    var scopeFactory = serviceProvider
                        .GetRequiredService<IServiceScopeFactory>();
                    serviceScope = scopeFactory.CreateScope();
                    serviceScopes[key] =
                        new WeakReference<IServiceScope>(serviceScope);
                }
                return serviceScope.ServiceProvider;
            }
        }
    }
}
