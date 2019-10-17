using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using System.CommandLine.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.CommandLine.Hosting
{
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = nameof(Microsoft.Extensions.DependencyInjection))]
    internal class InvocationLifetimeOptionsPostConfigure : IPostConfigureOptions<InvocationLifetimeOptions>
    {
        private readonly IConfiguration configuration;

        public InvocationLifetimeOptionsPostConfigure(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void PostConfigure(string name, InvocationLifetimeOptions options)
            => configuration.Bind("Lifetime", options);
    }
}
