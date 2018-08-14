using System;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// Provides extension methods for the nested service collections.
    /// </summary>
    public static class NestedServiceCollectionExtensions
    {
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
        private static INestedServiceCollection InternalChangeRootServicesAddBehavior(
            this INestedServiceCollection services,
            RootServicesAddBehavior behavior)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.RootServicesAddBehavior = behavior;
            return services;
        }

        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
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

        /// <summary>
        /// Changes the root service behaviour for subquent operations on the
        /// specified nested service collection.
        /// </summary>
        /// <param name="services">The nested service collection for which to change the inheritance behaviour. Must not be <c>null</c>.</param>
        /// <param name="behavior">The new behaviour when adding subsequent nested services.</param>
        /// <returns>The instance specified in the <paramref name="services"/> parameter, to allow to fluent API chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
        public static INestedServiceCollection ChangeRootServicesAddBehavior(
            this INestedServiceCollection services,
            RootServicesAddBehavior behavior)
            => InternalChangeRootServicesAddBehavior(services, behavior);

        /// <summary>
        /// Changes the root service behaviour for subquent operations on the
        /// specified nested service collection.
        /// </summary>
        /// <param name="services">The nested service collection for which to change the inheritance behaviour. Must not be <c>null</c>.</param>
        /// <param name="behavior">The new behaviour when adding subsequent nested services.</param>
        /// <returns>The instance specified in the <paramref name="services"/> parameter, to allow to fluent API chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
        public static INestedServiceCollection<TKey> ChangeRootServicesAddBehavior<TKey>(
            this INestedServiceCollection<TKey> services,
            RootServicesAddBehavior behavior)
        {
            InternalChangeRootServicesAddBehavior(services, behavior);
            return services;
        }

        /// <summary>
        /// Changes the root service behaviour for subquent operations on the
        /// specified nested service collection.
        /// </summary>
        /// <param name="services">The nested service collection for which to change the inheritance behaviour. Must not be <c>null</c>.</param>
        /// <param name="behavior">The new behaviour when adding subsequent nested services.</param>
        /// <returns>The instance specified in the <paramref name="services"/> parameter, to allow to fluent API chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
        public static INestedServiceCollection<TFamily, TKey> ChangeRootServicesAddBehavior<TFamily, TKey>(
            this INestedServiceCollection<TFamily, TKey> services,
            RootServicesAddBehavior behavior)
        {
            InternalChangeRootServicesAddBehavior(services, behavior);
            return services;
        }

        /// <summary>
        /// Configures the nested service collection using the specified behaviour
        /// when registring nested services.
        /// <para>The previous behaviour will be reset when this method returns.</para>
        /// </summary>
        /// <param name="services">The nested service collection to configure. Must not be <c>null</c>.</param>
        /// <param name="behavior">The inherited service behaviour to using during configuration.</param>
        /// <param name="configureServices">The configuration function to invoke to configure the service collection.</param>
        /// <returns>The instance specified in the <paramref name="services"/> parameter, to allow to fluent API chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
        public static INestedServiceCollection WithRootServicesAddBehavior(
            this INestedServiceCollection services,
            RootServicesAddBehavior behavior,
            Action<INestedServiceCollection> configureServices)
            => InternalWithRootServicesAddBehavior(services, behavior, configureServices);

        /// <summary>
        /// Configures the nested service collection using the specified behaviour
        /// when registring nested services.
        /// <para>The previous behaviour will be reset when this method returns.</para>
        /// </summary>
        /// <param name="services">The nested service collection to configure. Must not be <c>null</c>.</param>
        /// <param name="behavior">The inherited service behaviour to using during configuration.</param>
        /// <param name="configureServices">The configuration function to invoke to configure the service collection.</param>
        /// <returns>The instance specified in the <paramref name="services"/> parameter, to allow to fluent API chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
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

        /// <summary>
        /// Configures the nested service collection using the specified behaviour
        /// when registring nested services.
        /// <para>The previous behaviour will be reset when this method returns.</para>
        /// </summary>
        /// <param name="services">The nested service collection to configure. Must not be <c>null</c>.</param>
        /// <param name="behavior">The inherited service behaviour to using during configuration.</param>
        /// <param name="configureServices">The configuration function to invoke to configure the service collection.</param>
        /// <returns>The instance specified in the <paramref name="services"/> parameter, to allow to fluent API chaining.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <c>null</c>.</exception>
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
