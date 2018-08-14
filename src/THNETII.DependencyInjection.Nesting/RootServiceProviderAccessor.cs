using System;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// Provides access to the Parent service container of a nested dependency
    /// injection container.
    /// </summary>
    public class RootServiceProviderAccessor
    {
        /// <summary>
        /// Creates a new Root Service Provider Accesor instance with the
        /// specified root service container.
        /// </summary>
        /// <param name="rootServiceProvider">
        /// The service provider that is the root of the nested service container.
        /// </param>
        public RootServiceProviderAccessor(IServiceProvider rootServiceProvider)
        {
            RootServiceProvider = rootServiceProvider;
        }

        /// <summary>
        /// Gets the parent dependency injection container.
        /// </summary>
        public virtual IServiceProvider RootServiceProvider { get; }
    }
}
