using Microsoft.Extensions.DependencyInjection;

using System;

using THNETII.Common;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// Provides extension methods for nested services on <see cref="IServiceProvider"/>
    /// instances.
    /// </summary>
    public static class NestedServicesServiceProviderExtensions
    {
        /// <summary>
        /// Returns a nested service provider with the specified key.
        /// </summary>
        /// <param name="serviceProvider">
        /// The parent service provider that supports nested services.
        /// Must not be <see langword="null"/>.
        /// </param>
        /// <param name="key">The unique identifier of the nested service provider.</param>
        /// <returns>An <see cref="IServiceProvider"/> instance that resolves services nested under the specified parent provider.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
        public static IServiceProvider GetNestedServiceProvider(
            this IServiceProvider serviceProvider, string key)
            => GetNestedServiceProvider<object>(serviceProvider, key);

        /// <summary>
        /// Returns a nested service provider with the specified key.
        /// </summary>
        /// <param name="serviceProvider">
        /// The parent service provider that supports nested services.
        /// Must not be <see langword="null"/>.
        /// </param>
        /// <param name="key">The unique identifier of the nested service provider.</param>
        /// <returns>An <see cref="IServiceProvider"/> instance that resolves services nested under the specified parent provider.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
        public static IServiceProvider GetNestedServiceProvider<TFamily>(
            this IServiceProvider serviceProvider, string key)
            => GetNestedServiceProvider<TFamily, string>(serviceProvider, key);

        /// <summary>
        /// Returns a nested service provider with the specified key.
        /// </summary>
        /// <param name="serviceProvider">
        /// The parent service provider that supports nested services.
        /// Must not be <see langword="null"/>.
        /// </param>
        /// <param name="key">The unique key of the nested service provider.</param>
        /// <returns>An <see cref="IServiceProvider"/> instance that resolves services nested under the specified parent provider.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
        public static IServiceProvider GetNestedServiceProvider<TFamily, TKey>(
            this IServiceProvider serviceProvider, TKey key)
        {
            return serviceProvider.ThrowIfNull(nameof(serviceProvider))
                .GetRequiredService<INestedServicesContainer<TFamily, TKey>>()
                .GetServiceProvider(key);
        }
    }
}
