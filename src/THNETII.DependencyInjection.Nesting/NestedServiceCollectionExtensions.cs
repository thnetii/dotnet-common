using System;

namespace THNETII.DependencyInjection.Nesting
{
    public static class NestedServiceCollectionExtensions
    {
        private static INestedServiceCollection InternalChangeRootServicesAddBehavior(
            this INestedServiceCollection services,
            RootServicesAddBehavior behavior)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.RootServicesAddBehavior = behavior;
            return services;
        }

        private static INestedServiceCollection InternalWithRootServicesAddBehavior(
            this INestedServiceCollection services,
            RootServicesAddBehavior behavior,
            Action<INestedServiceCollection> configureServices)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            var oldBehavior = services.RootServicesAddBehavior;
            services.RootServicesAddBehavior = behavior;
            configureServices?.Invoke(services);
            services.RootServicesAddBehavior = oldBehavior;
            return services;
        }

        public static INestedServiceCollection ChangeRootServicesAddBehavior(
            this INestedServiceCollection services,
            RootServicesAddBehavior behavior)
            => InternalChangeRootServicesAddBehavior(services, behavior);

        public static INestedServiceCollection<TKey> ChangeRootServicesAddBehavior<TKey>(
            this INestedServiceCollection<TKey> services,
            RootServicesAddBehavior behavior)
        {
            InternalChangeRootServicesAddBehavior(services, behavior);
            return services;
        }

        public static INestedServiceCollection<TFamily, TKey> ChangeRootServicesAddBehavior<TFamily, TKey>(
            this INestedServiceCollection<TFamily, TKey> services,
            RootServicesAddBehavior behavior)
        {
            InternalChangeRootServicesAddBehavior(services, behavior);
            return services;
        }

        public static INestedServiceCollection WithRootServicesAddBehavior(
            this INestedServiceCollection services,
            RootServicesAddBehavior behavior,
            Action<INestedServiceCollection> configureServices)
            => InternalWithRootServicesAddBehavior(services, behavior, configureServices);

        public static INestedServiceCollection<TKey> WithRootServicesAddBehavior<TKey>(
            this INestedServiceCollection<TKey> services,
            RootServicesAddBehavior behavior,
            Action<INestedServiceCollection<TKey>> configureServices)
        {
            Action<INestedServiceCollection> internalConfigureServices;
            if (configureServices == null)
                internalConfigureServices = null;
            else
            {
                internalConfigureServices = s =>
                    configureServices((INestedServiceCollection<TKey>)s);
            }
            InternalWithRootServicesAddBehavior(services, behavior,
                internalConfigureServices);
            return services;
        }

        public static INestedServiceCollection<TFamily, TKey> WithRootServicesAddBehavior<TFamily, TKey>(
            this INestedServiceCollection<TFamily, TKey> services,
            RootServicesAddBehavior behavior,
            Action<INestedServiceCollection<TFamily, TKey>> configureServices)
        {
            Action<INestedServiceCollection> internalConfigureServices;
            if (configureServices == null)
                internalConfigureServices = null;
            else
            {
                internalConfigureServices = s =>
                    configureServices((INestedServiceCollection<TFamily, TKey>)s);
            }
            InternalWithRootServicesAddBehavior(services, behavior,
                internalConfigureServices);
            return services;
        }
    }
}
