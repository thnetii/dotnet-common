using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace THNETII.DependencyInjection.Nesting
{
    public static class NestedServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddNestedServices(
            this IServiceCollection rootServices, string key,
            Action<INestedServiceCollection<string>> configureServices)
            => AddNestedServices<object>(rootServices, key, configureServices);

        public static IServiceCollection AddNestedServices(
            this IServiceCollection rootServices, string key, bool inheritRoot,
            Action<INestedServiceCollection<string>> configureServices)
            => AddNestedServices<object>(rootServices, key, inheritRoot,
                configureServices);

        public static IServiceCollection AddNestedServices<TFamily>(
            this IServiceCollection rootServices, string key,
            Action<INestedServiceCollection<TFamily, string>> configureServices)
            => AddNestedServices<TFamily, string>(rootServices, key,
                configureServices);

        public static IServiceCollection AddNestedServices<TFamily>(
            this IServiceCollection rootServices, string key, bool inheritRoot,
            Action<INestedServiceCollection<TFamily, string>> configureServices)
            => AddNestedServices<TFamily, string>(rootServices, key,
                inheritRoot, configureServices);

        public static IServiceCollection AddNestedServices<TFamily, TKey>(
            this IServiceCollection rootServices, TKey key,
            Action<INestedServiceCollection<TFamily, TKey>> configureServices)
            => AddNestedServices(rootServices, key, false,
                configureServices);

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
