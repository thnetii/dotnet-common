using Microsoft.Extensions.DependencyInjection;

using System;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// Provides extension methods to <see cref="IServiceCollection"/> instances
    /// to easily add nested service collections.
    /// </summary>
    public static class NestedServicesServiceCollectionExtensions
    {
        /// <summary>
        /// Add a nested service collection with the specified identifier to the
        /// service collection.
        /// </summary>
        /// <param name="rootServices">The service collection to use as the parent of the nested service collection.</param>
        /// <param name="key">The unique identifier to use for the nested services.</param>
        /// <param name="configureServices">The actions to execute to configure the nested service collection.</param>
        /// <returns>The instance specified in the <paramref name="rootServices"/> parameter, to allow to fluent API chaining.</returns>
        public static IServiceCollection AddNestedServices(
            this IServiceCollection rootServices, string key,
            Action<INestedServiceCollection<string>> configureServices)
            => AddNestedServices<object>(rootServices, key, configureServices);

        /// <summary>
        /// Add a nested service collection with the specified identifier to the
        /// service collection.
        /// </summary>
        /// <param name="rootServices">The service collection to use as the parent of the nested service collection.</param>
        /// <param name="key">The unique identifier to use for the nested services.</param>
        /// <param name="inheritRoot">A value indicating whether the services registered in <paramref name="rootServices"/> should be inherited by the nested service collection.</param>
        /// <param name="configureServices">The actions to execute to configure the nested service collection.</param>
        /// <returns>The instance specified in the <paramref name="rootServices"/> parameter, to allow to fluent API chaining.</returns>
        public static IServiceCollection AddNestedServices(
            this IServiceCollection rootServices, string key, bool inheritRoot,
            Action<INestedServiceCollection<string>> configureServices)
            => AddNestedServices<object>(rootServices, key, inheritRoot,
                configureServices);

        /// <summary>
        /// Add a nested service collection with the specified identifier to the
        /// service collection.
        /// </summary>
        /// <param name="rootServices">The service collection to use as the parent of the nested service collection.</param>
        /// <param name="key">The unique identifier to use for the nested services.</param>
        /// <param name="configureServices">The actions to execute to configure the nested service collection.</param>
        /// <returns>The instance specified in the <paramref name="rootServices"/> parameter, to allow to fluent API chaining.</returns>
        public static IServiceCollection AddNestedServices<TFamily>(
            this IServiceCollection rootServices, string key,
            Action<INestedServiceCollection<TFamily, string>> configureServices)
            => AddNestedServices<TFamily, string>(rootServices, key,
                configureServices);

        /// <summary>
        /// Add a nested service collection with the specified identifier to the
        /// service collection.
        /// </summary>
        /// <param name="rootServices">The service collection to use as the parent of the nested service collection.</param>
        /// <param name="key">The unique identifier to use for the nested services.</param>
        /// <param name="inheritRoot">A value indicating whether the services registered in <paramref name="rootServices"/> should be inherited by the nested service collection.</param>
        /// <param name="configureServices">The actions to execute to configure the nested service collection.</param>
        /// <returns>The instance specified in the <paramref name="rootServices"/> parameter, to allow to fluent API chaining.</returns>
        public static IServiceCollection AddNestedServices<TFamily>(
            this IServiceCollection rootServices, string key, bool inheritRoot,
            Action<INestedServiceCollection<TFamily, string>> configureServices)
            => AddNestedServices<TFamily, string>(rootServices, key,
                inheritRoot, configureServices);

        /// <summary>
        /// Add a nested service collection with the specified identifier to the
        /// service collection.
        /// </summary>
        /// <param name="rootServices">The service collection to use as the parent of the nested service collection.</param>
        /// <param name="key">The unique identifier to use for the nested services.</param>
        /// <param name="configureServices">The actions to execute to configure the nested service collection.</param>
        /// <returns>The instance specified in the <paramref name="rootServices"/> parameter, to allow to fluent API chaining.</returns>
        public static IServiceCollection AddNestedServices<TFamily, TKey>(
            this IServiceCollection rootServices, TKey key,
            Action<INestedServiceCollection<TFamily, TKey>> configureServices)
            => AddNestedServices(rootServices, key, false,
                configureServices);

        /// <summary>
        /// Add a nested service collection with the specified identifier to the
        /// service collection.
        /// </summary>
        /// <param name="rootServices">The service collection to use as the parent of the nested service collection.</param>
        /// <param name="key">The unique identifier to use for the nested services.</param>
        /// <param name="inheritRoot">A value indicating whether the services registered in <paramref name="rootServices"/> should be inherited by the nested service collection.</param>
        /// <param name="configureServices">The actions to execute to configure the nested service collection.</param>
        /// <returns>The instance specified in the <paramref name="rootServices"/> parameter, to allow to fluent API chaining.</returns>
        public static IServiceCollection AddNestedServices<TFamily, TKey>(
            this IServiceCollection rootServices, TKey key, bool inheritRoot,
            Action<INestedServiceCollection<TFamily, TKey>> configureServices)
        {
            var nestedServiceCollection = new NestedServiceCollection<TFamily, TKey>(
                key, inheritRoot ? rootServices : null);
            configureServices?.Invoke(nestedServiceCollection);

            rootServices.AddSingleton<INestedServiceCollection>(nestedServiceCollection);
            rootServices.AddSingleton<INestedServiceCollection<TKey>>(nestedServiceCollection);
            rootServices.AddSingleton<INestedServiceCollection<TFamily, TKey>>(nestedServiceCollection);
            rootServices.AddSingleton<NestedSingletonServicesManager<TFamily, TKey>>();
            rootServices.AddScoped<INestedServicesContainer<TFamily, TKey>, NestedScopedServicesManager<TFamily, TKey>>();

            return rootServices;
        }
    }
}
