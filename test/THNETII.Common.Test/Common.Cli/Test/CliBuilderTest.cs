using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace THNETII.Common.Cli.Test
{
    public class CliBuilderTest
    {
        [Fact]
        public void CliBuilderApplicationHasNameByDefault()
        {
            CommandLineApplication cliapp = new CliBuilder<CliBuilderTestCommand>()
                .Build();
            Assert.False(string.IsNullOrWhiteSpace(cliapp.Name));
        }

        [Fact]
        public void CliBuilderApplicationHasFullNameByDefault()
        {
            CommandLineApplication cliapp = new CliBuilder<CliBuilderTestCommand>()
                .Build();
            Assert.False(string.IsNullOrWhiteSpace(cliapp.FullName));
        }

        [Fact]
        public void CliBuilderApplicationHasDescriptionByDefault()
        {
            CommandLineApplication cliapp = new CliBuilder<CliBuilderTestCommand>()
                .Build();
            Assert.False(string.IsNullOrWhiteSpace(cliapp.Description));
        }

        [Fact]
        public void CliBuilderApplicationHasExecuteFunctionByDefault()
        {
            CommandLineApplication cliapp = new CliBuilder<CliBuilderTestCommand>()
               .Build();
            Assert.NotNull(cliapp.Invoke);
        }

        [Fact]
        public void CliBuilderApplicationHasHelpOption()
        {
            CommandLineApplication cliapp = new CliBuilder<CliBuilderTestCommand>()
                .AddHelpOption()
                .Build();
            Assert.NotNull(cliapp.OptionHelp);
            Assert.Equal(CliBuilder<CliBuilderTestCommand>.DefaultHelpTemplate, cliapp.OptionHelp.Template);
        }

        [Fact]
        public void CliBuilderApplicationHasVersionOption()
        {
            CommandLineApplication cliapp = new CliBuilder<CliBuilderTestCommand>()
                .AddVersionOption()
                .Build();
            Assert.NotNull(cliapp.OptionVersion);
            Assert.Equal(CliBuilder<CliBuilderTestCommand>.DefaultVersionTemplate, cliapp.OptionVersion.Template);
        }

        private class CliBuilderTestCommand : CliCommand
        {
            public override int Run(CommandLineApplication app) => 0;
        }
    }
}
