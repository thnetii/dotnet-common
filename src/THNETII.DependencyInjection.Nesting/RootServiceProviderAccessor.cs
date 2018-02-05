using System;

namespace THNETII.DependencyInjection.Nesting
{
    public class RootServiceProviderAccessor
    {
        public RootServiceProviderAccessor(IServiceProvider rootServiceProvider)
        {
            RootServiceProvider = rootServiceProvider;
        }

        public virtual IServiceProvider RootServiceProvider { get; }
    }
}
