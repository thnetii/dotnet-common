using Microsoft.Extensions.DependencyInjection;
using System;
using THNETII.Common;

namespace THNETII.DependencyInjection.Nesting
{
    public static class NestedServicesServiceProviderExtensions
    {
        public static IServiceProvider GetNestedServiceProvider(
            this IServiceProvider serviceProvider, string key)
            => GetNestedServiceProvider<object>(serviceProvider, key);

        public static IServiceProvider GetNestedServiceProvider<TFamily>(
            this IServiceProvider serviceProvider, string key)
            => GetNestedServiceProvider<TFamily, string>(serviceProvider, key);

        public static IServiceProvider GetNestedServiceProvider<TFamily, TKey>(
            this IServiceProvider serviceProvider, TKey key)
        {
            return serviceProvider.ThrowIfNull(nameof(serviceProvider))
                .GetRequiredService<INestedServicesContainer<TFamily, TKey>>()
                .GetServiceProvider(key);
        }
    }
}
