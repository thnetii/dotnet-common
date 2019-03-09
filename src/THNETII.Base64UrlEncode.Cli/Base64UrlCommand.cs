using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using THNETII.Common;

namespace THNETII.Base64UrlEncode
{
    public class Base64UrlCommand : RootCommand
    {
        private static readonly string description = typeof(Program).Assembly
            .GetCustomAttribute<AssemblyDescriptionAttribute>()?
            .Description;

        private readonly Option decodeOption = new Option(
            new string[] { "-d", "--decode" },
            "decode data",
            new Argument<bool>()
            );
        private readonly Option ignoreGarbageOption = new Option(
            new string[] { "-i", "--ignore-garbage" },
            "when decoding, ignore non-alphabet characters",
            new Argument<bool>()
            );
        private readonly Option wrapOption = new Option(
            new string[] { "-w", "--wrap" },
            "wrap encoded lines after COLS character (default 76).  Use 0 to disable line wrapping",
            GetWrapArgument()
            );
        private readonly Option fileOption = new Option(
            new string[] { "-f", "--file" },
            "Encode or decode contents of FILE",
            new Argument<FileInfo>() { Arity = ArgumentArity.ZeroOrOne, Name = "FILE" }.ExistingOnly()
            );
        private readonly Option charsetOption = new Option(
            new string[] { "-c", "--charset" },
            "Use CHARSET when decoding data from a file (Default: UTF-8).",
            GetEncodingArgument()
            );
        private readonly Option bufferSizeOption = new Option(
            new string[] { "-b", "--buffer" },
            "Use SIZE as the read-buffer size (in bytes). (Default: 4096)",
            new Argument<int>(4096) { Name = "SIZE", Description = "Number of bytes to use for read buffer" }
            );
        private readonly Argument<string> dataArgument = GetDataArgument();

        private static Argument<int> GetWrapArgument()
        {
            var arg = new Argument<int>(76)
            {
                Name = "COLS"
            };
            arg.Description = $"Number of characters per line (default {arg.GetDefaultValue()})";
            arg.AddValidator(symbol => symbol.Arguments.Select(s =>
            {
                int v;
                try { v = int.Parse(s, CultureInfo.CurrentCulture); }
                catch (Exception e) { return e.ToString(); }
                if (v < 0)
                    return $"Argument '{s}' for option '{symbol.Token}' is invalid. Expected a non-negative integer value.";
                return null;
            }).Where(msg => !string.IsNullOrWhiteSpace(msg)).FirstOrDefault());
            return arg;
        }

        private static Argument<Encoding> GetEncodingArgument()
        {
            var arg = new Argument<Encoding>(ConvertToEncoding)
            {
                Arity = ArgumentArity.ZeroOrOne,
                Name = "CHARSET",
                Description = "IANA charset name (default: UTF-8)"
            };
            arg.AddSuggestions(Encoding.GetEncodings().Select(enc => enc.Name).ToArray());
            arg.SetDefaultValue(Encoding.UTF8);
            return arg;

            ArgumentResult ConvertToEncoding(SymbolResult symbol)
            {
                if (symbol.Arguments.FirstOrDefault().TryNotNullOrWhiteSpace(out string charset))
                {
                    try
                    {
                        var encoding = Encoding.GetEncoding(charset);
                        return ArgumentResult.Success(encoding);
                    }
                    catch (ArgumentException argExcept)
                    {
                        return ArgumentResult.Failure(argExcept.ToString());
                    }
                }
                return ArgumentResult.Success(Encoding.UTF8);
            }
        }

        private static Argument<string> GetDataArgument()
        {
            var arg = new Argument<string>
            {
                Name = "DATA",
                Arity = ArgumentArity.ZeroOrOne,
                Description = "Encode or decode DATA"
            };

            return arg;
        }

        public Base64UrlCommand() : base()
        {
            Description = description;
            AddOption(decodeOption);
            AddOption(ignoreGarbageOption);
            AddOption(wrapOption);
            AddOption(fileOption);
            AddOption(charsetOption);
            AddOption(bufferSizeOption);
            Argument = dataArgument;

            Handler = CommandHandler.Create((Func<ParseResult, IConsole, Task<int>>)InvokeAsync);
        }

        private Task<int> InvokeAsync(ParseResult parseResult, IConsole console)
        {
            bool decode = GetValueOrDefaultFor<bool>(decodeOption);
            bool ignoreGarbage = GetValueOrDefaultFor<bool>(ignoreGarbageOption);
            int wrap = GetValueOrDefaultFor<int>(wrapOption);
            FileInfo file = GetValueOrDefaultFor<FileInfo>(fileOption);
            string data = GetValueOrDefaultFor<string>(this);
            Encoding encoding = GetValueOrDefaultFor<Encoding>(charsetOption);
            int bufferSize = GetValueOrDefaultFor<int>(bufferSizeOption);

            return Program.InvokeAsync(console, decode, ignoreGarbage, wrap, file, data, encoding, bufferSize);

            T GetValueOrDefaultFor<T>(ISymbol symbol)
            {
                return parseResult.FindResultFor(symbol)
                    .GetValueOrDefault<T>();
            }
        }
    }
}
