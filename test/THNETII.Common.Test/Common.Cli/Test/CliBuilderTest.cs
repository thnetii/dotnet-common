using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

using static THNETII.Common.Cli.CliBuilder<THNETII.Common.Cli.CliCommand>;

namespace THNETII.Common.Cli.Test
{
    public class CliBuilderTest
    {

        const int DefaultReturnValue = 4242;
        const string TestConfigurationKey = "TEST";
        const string TestConfigurationValue = "TEST-CONFIG-VALUE";


        private class CliTestCommand : CliCommand
        {
            public override int Run(CommandLineApplication app) => DefaultReturnValue;
        }

        private class CliTestCommandWithConfiguration : CliCommand
        {
            public CliTestCommandWithConfiguration(IConfiguration configuration) : base(configuration) { }

            public override int Run(CommandLineApplication app)
            {
                Assert.NotNull(Configuration);
                return DefaultReturnValue;
            }
        }

        private class CliTestCommandWithConfigurationValue : CliTestCommandWithConfiguration
        {
            public CliTestCommandWithConfigurationValue(IConfiguration configuration) : base(configuration) { }

            public override int Run(CommandLineApplication app)
            {
                Assert.Equal(TestConfigurationValue, Configuration?[TestConfigurationKey]);
                return base.Run(app);
            }
        }

        private class TestService { }

        private class CliTestCommandWithTestService : CliTestCommand
        {
            public TestService Service { get; }

            public CliTestCommandWithTestService(TestService service) : base()
            {
                Service = service;
            }

            public override int Run(CommandLineApplication app)
            {
                Assert.NotNull(Service);
                return DefaultReturnValue;
            }
        }

        [Fact]
        [SuppressMessage("Microsoft.Usage", "CA1806", Justification = "Usage of constructed instance not relevant")]
        public void ConstructorWithNoArguments()
            => new CliBuilder<CliTestCommand>();

        [Fact]
        public void BuildWithNoBuilderSteps()
        {
            var builder = new CliBuilder<CliTestCommand>();
            var cliapp = builder.Build();
            Assert.NotNull(cliapp);
            Assert.Empty(cliapp.Arguments);
            Assert.Empty(cliapp.Commands);
            Assert.Empty(cliapp.Options);
            Assert.Null(cliapp.OptionHelp);
            Assert.Null(cliapp.OptionVersion);
            Assert.Equal(DefaultReturnValue, cliapp.Execute());
        }

        [Fact]
        public void BuildWithConfiguration()
        {
            var builder = new CliBuilder<CliTestCommandWithConfiguration>();
            var cliapp = builder.Build();
            Assert.NotNull(cliapp);
            Assert.Equal(DefaultReturnValue, cliapp.Execute());
        }

        [Fact]
        public void BuildWithConfigurationWithKeyValuePair()
        {
            var builder = new CliBuilder<CliTestCommandWithConfigurationValue>();
            builder.AddConfiguration(config =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(TestConfigurationKey, TestConfigurationValue)
                });
            });
            var cliapp = builder.Build();
            Assert.NotNull(cliapp);
            Assert.Equal(DefaultReturnValue, cliapp.Execute());
        }

        [Fact]
        public void AddConfigurationReturnsSameBuilderInstance()
        {
            var builder = new CliBuilder<CliCommand>();
            Assert.Same(builder, builder.AddConfiguration(_ => { }));
        }

        [Fact]
        public void AddConfigurationAcceptsNullArgument()
        {
            var builder = new CliBuilder<CliCommand>();
            builder.AddConfiguration(null);
        }

        [Fact]
        public void ConfigureServicesReturnsSameBuilderInstance()
        {
            var builder = new CliBuilder<CliCommand>();
            Assert.Same(builder, builder.ConfigureServices(_ => { }));
        }

        [Fact]
        public void ConfigureServicesAcceptsNullArgument()
        {
            var builder = new CliBuilder<CliCommand>();
            builder.ConfigureServices((Action<IServiceCollection>)null);
        }

        [Fact]
        public void ConfigureServicesWithConfigurationReturnsSameBuilderInstance()
        {
            var builder = new CliBuilder<CliCommand>();
            Assert.Same(builder, builder.ConfigureServices((_1, _2) => { }));
        }

        [Fact]
        public void ConfigureServicesWithConfigurationAcceptsNullArgument()
        {
            var builder = new CliBuilder<CliCommand>();
            builder.ConfigureServices((Action<IConfiguration, IServiceCollection>)null);
        }

        [Fact]
        public void AddHelpOptionCreatesHelpOption()
        {
            var builder = new CliBuilder<CliCommand>();
            builder.AddHelpOption();
            var cliapp = builder.Build();
            Assert.NotNull(cliapp.OptionHelp);
        }

        [Fact]
        public void AddHelpOptionCreatesHelpOptionWithSpecifiedTemplate()
        {
            const string helpTemplate = "--test-help";
            var builder = new CliBuilder<CliCommand>();
            builder.AddHelpOption(helpTemplate);
            var cliapp = builder.Build();
            Assert.NotNull(cliapp.OptionHelp);
            Assert.Equal(helpTemplate, cliapp.OptionHelp.Template);
        }

        [Fact]
        public void AddVersionOptionCreatesVersionOptionWithStrings()
        {
            var builder = new CliBuilder<CliCommand>();
            builder.AddVersionOption("v1.2.3-test", "v1.2.3.4");
            var cliapp = builder.Build();
            Assert.NotNull(cliapp.OptionVersion);
            Assert.NotNull(cliapp.ShortVersionGetter);
            Assert.NotNull(cliapp.LongVersionGetter);
        }
    }
}
