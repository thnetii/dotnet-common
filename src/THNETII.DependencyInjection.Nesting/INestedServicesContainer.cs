using System;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// Specifies the contract for an instance of a nested dependency injection
    /// service container.
    /// </summary>
    /// <typeparam name="TFamily">The type of the family of service descriptors.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key that distingueshes the nested service collection
    /// from its siblings.
    /// </typeparam>
    public interface INestedServicesContainer<TFamily, TKey>
    {
        /// <summary>
        /// Retrieves the service provider using the service container
        /// with the specified key.
        /// </summary>
        /// <param name="key">The key of the service container to use.</param>
        /// <returns>
        /// An <see cref="IServiceProvider"/> instance resolving the services
        /// registered to the container with the specified key.
        /// </returns>
        IServiceProvider GetServiceProvider(TKey key);
    }
}
