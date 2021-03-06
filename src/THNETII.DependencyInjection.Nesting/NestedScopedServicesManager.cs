using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using THNETII.Common;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// A scoped dependency injection service that enforces scoping
    /// boundaries across nested dependency injection containers.
    /// </summary>
    /// <typeparam name="TFamily">The type for the family of the nested containers this manager is managing.</typeparam>
    /// <typeparam name="TKey">The type of the key for the nested service container the instance is managing.</typeparam>
    [SuppressMessage(category: null, "CA1063", Justification = "No native resources to dispose.")]
    [SuppressMessage(category: null, "CA1816", Justification = "Call to SuppressFinalize not required.")]
    public class NestedScopedServicesManager<TFamily, TKey>
        : INestedServicesContainer<TFamily, TKey>, IDisposable
    {
        private readonly NestedSingletonServicesManager<TFamily, TKey> singletonManager;
        private readonly IDictionary<TKey, WeakReference<IServiceScope>> serviceScopes;

        /// <summary>
        /// Creates a new scope manager instance using the specified parent
        /// service container and the related singleton manager.
        /// </summary>
        /// <param name="rootServiceProvider">The parent container that is shared by the nested containers that are to be managed.</param>
        /// <param name="singletonManager">The singleton manager managing the singleton services for the nested containers. Must not be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="singletonManager"/> is <see langword="null"/>.</exception>
        public NestedScopedServicesManager(
            IServiceProvider rootServiceProvider,
            NestedSingletonServicesManager<TFamily, TKey> singletonManager)
        {
            var keyComparer = NestingKeyComparer<TKey>.
                GetNestingKeyComparer(rootServiceProvider);
            serviceScopes = new Dictionary<TKey, WeakReference<IServiceScope>>(keyComparer);
            this.singletonManager = singletonManager.ThrowIfNull(nameof(singletonManager));
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
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

        /// <inheritdoc cref="INestedServicesContainer{TFamily, TKey}.GetServiceProvider(TKey)" />
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
