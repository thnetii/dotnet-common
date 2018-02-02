using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace THNETII.DependencyInjection.Nesting
{
    public static class NestedServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddNestedServices(
            this IServiceCollection rootServices,
            string key,
            Action<INestedServiceCollection> configureServices)
        {
            return AddNestedServices<string>(
                rootServices,
                key,
                StringComparer.OrdinalIgnoreCase,
                configureServices
                );
        }

        public static IServiceCollection AddNestedServices<T>(
            this IServiceCollection rootServices,
            string key,
            Action<INestedServiceCollection> configureServices)
            => AddNestedServices<T>(rootServices, key,
                StringComparer.OrdinalIgnoreCase,
                configureServices);

        public static IServiceCollection AddNestedServices<T>(
            this IServiceCollection rootServices,
            string key, IEqualityComparer<string> keyComparer,
            Action<INestedServiceCollection> configureServices)
        {
            throw new NotImplementedException();
            return rootServices;
        }
    }
}
