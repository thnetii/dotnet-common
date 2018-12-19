using McMaster.Extensions.CommandLineUtils;
using System.Reflection;

namespace THNETII.Base64UrlEncode
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var programAssembly = typeof(Program).Assembly;
            var app = new CommandLineApplication
            {
                AllowArgumentSeparator = true,
                FullName = programAssembly.GetName().Name,
                Description = programAssembly
                    .GetCustomAttribute<AssemblyDescriptionAttribute>()?
                    .Description
            };
            app.VersionOptionFromAssemblyAttributes(programAssembly);
            app.HelpOption();

            var decode = app.Option("-d|--decode", "Decode input", CommandOptionType.NoValue);
            var stringEncoding = app.Option("--string-encoding", "Byte encoding (default: UTF-8)", CommandOptionType.SingleValue);
            
        }
    }
}
