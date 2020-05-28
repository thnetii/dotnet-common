using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace THNETII.TypeConverter
{
    /// <summary>
    /// Provides static conversion methods using the <see cref="NumberStringConverter{T}"/>
    /// class for the built-in primitive numeric types.
    /// </summary>
    /// <seealso cref="NumberStringConverter{T}"/>
    public static class NumberStringConverter
    {
        /// <summary>
        /// Gets the primitive number string converters that use the <see cref="NumberFormatInfo"/>
        /// object for the current culture for conversion.
        /// </summary>
        public static PrimitiveNumberStringConverter CurrentCulture { get; } =
            new PrimitiveNumberStringConverter();

        /// <summary>
        /// Gets the primitive number string converters that use the <see cref="NumberFormatInfo"/>
        /// object for the invariant culture for conversion.
        /// </summary>
        public static PrimitiveNumberStringConverter InvariantCulture { get; } =
            new PrimitiveNumberStringConverter(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Provides conversion methods to convert strings to numeric values using
    /// <see cref="NumberStyles.Any"/> or <see cref="NumberStyles.HexNumber"/>.
    /// </summary>
    /// <typeparam name="T">The numeric struct-type to convert to and from.</typeparam>
    /// <remarks>
    /// When parsing, the specified string is parsing using both numeric (decimal) and
    /// hexadecimal number parsing.
    /// <para>Hexadecimal parsing is not supported for floating-point types.</para>
    /// </remarks>
    public class NumberStringConverter<T> where T : struct, IFormattable
    {
        private readonly NumberStylesParseFunc<T> parse;
        private readonly NumberStylesTryParseFunc<T> tryParse;
        private readonly IFormatProvider? formatProvider;

        /// <summary>
        /// Initializes a new <see cref="NumberStringConverter{T}"/> using the
        /// specified parser and try-parse functions and optional format provider.
        /// </summary>
        /// <param name="parse">The parse-function to use to parse string values to values of <typeparamref name="T"/>.</param>
        /// <param name="tryParse">The parse-functions to use to attempt parsing of string values to values of <typeparamref name="T"/>.</param>
        /// <param name="formatProvider">Optional. An object that supplies culture-specific information about the format of the strings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parse"/> or <paramref name="tryParse"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// The <paramref name="formatProvider"/> parameter is an <see cref="IFormatProvider"/> implementation, such as
        /// a <see cref="NumberFormatInfo"/> or <see cref="CultureInfo"/> object. The <paramref name="formatProvider"/>
        /// parameter supplies culture-specific information used in parsing and stringification.
        /// If <paramref name="formatProvider"/> is <see langword="null"/>, the <see cref="NumberFormatInfo"/> object
        /// for the current culture is used.
        /// </remarks>
        /// <seealso cref="int.Parse(string, NumberStyles, IFormatProvider)"/>
        /// <seealso cref="int.TryParse(string, NumberStyles, IFormatProvider, out int)"/>
        public NumberStringConverter(NumberStylesParseFunc<T> parse,
            NumberStylesTryParseFunc<T> tryParse, IFormatProvider? formatProvider = default)
        {
            this.parse = parse ?? throw new ArgumentNullException(nameof(parse));
            this.tryParse = tryParse ?? throw new ArgumentNullException(nameof(tryParse));
            this.formatProvider = formatProvider;
        }

        /// <summary>
        /// Converts the specified string to a value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <returns>
        /// A value of <typeparamref name="T"/> that is equivalent to the string
        /// specified in <paramref name="s"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
        /// <exception cref="OverflowException">The number represented by <paramref name="s"/> cannot losslessly fit into a value of type <typeparamref name="T"/>.</exception>
        /// <seealso cref="int.Parse(string, NumberStyles, IFormatProvider)"/>
        public T Parse(string s)
        {
            if (TryParse(s, out T value))
                return value;
            return parse(s, NumberStyles.Number, formatProvider);
        }

        /// <summary>
        /// Converts the specified string to a value of type <typeparamref name="T"/>
        /// or returns the default value for <typeparamref name="T"/> if the string
        /// cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <returns>
        /// A value of <typeparamref name="T"/> that is equivalent to the string
        /// specified in <paramref name="s"/> or the default value for <typeparamref name="T"/> if
        /// <paramref name="s"/> cannot be converted into a value of <typeparamref name="T"/>.
        /// </returns>
        public T ParseOrDefault(string? s) => ParseOrDefault(s, default(T));

        /// <summary>
        /// Converts the specified string to a value of type <typeparamref name="T"/>
        /// or returns the a default value if the string cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="default">The value to return if <paramref name="s"/> cannot be converted to a value of <typeparamref name="T"/>.</param>
        /// <returns>
        /// A value of <typeparamref name="T"/> that is equivalent to the string
        /// specified in <paramref name="s"/> or <paramref name="default"/> if
        /// <paramref name="s"/> cannot be converted into a value of <typeparamref name="T"/>.
        /// </returns>
        public T ParseOrDefault(string? s, T @default) =>
            TryParse(s, out T value) ? value : @default;

        /// <summary>
        /// Converts the specified string to a value of type <typeparamref name="T"/>
        /// or constructs a default value if the string cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="defaultFactory">The function to invoke if <paramref name="s"/> cannot be converted to a value of <typeparamref name="T"/>.</param>
        /// <returns>
        /// A value of <typeparamref name="T"/> that is equivalent to the string
        /// specified in <paramref name="s"/> or value returned from invoking <paramref name="defaultFactory"/> if
        /// <paramref name="s"/> cannot be converted into a value of <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public T ParseOrDefault(string? s, Func<T> defaultFactory)
        {
            if (TryParse(s, out T value))
                return value;
            if (defaultFactory is null)
                throw new ArgumentNullException(nameof(defaultFactory));
            return defaultFactory();
        }

        /// <summary>
        /// Converts the specified string to a value of type <typeparamref name="T"/>
        /// or returns <see langword="null"/> if the string cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <returns>
        /// A value of <typeparamref name="T"/> that is equivalent to the string
        /// specified in <paramref name="s"/> or <see langword="null"/> if
        /// <paramref name="s"/> cannot be converted into a value of <typeparamref name="T"/>.
        /// </returns>
        public T? ParseOrNull(string? s) =>
            TryParse(s, out T value) ? (T?)value : null;

        /// <summary>
        /// Tries to convert the specified string to a value of <typeparamref name="T"/>.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise the default value of type <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public bool TryParse(string? s, out T value)
        {
            if (tryParse(s, NumberStyles.Number, formatProvider, out value))
                return true;
            ReadOnlySpan<char> hexPrefixSpan = "0x".AsSpan();
            ReadOnlySpan<char> startTrimmed = s.AsSpan().TrimStart();
            var hasHexPrefix = startTrimmed.StartsWith(hexPrefixSpan, StringComparison.OrdinalIgnoreCase);
            if (hasHexPrefix)
            {
                s = startTrimmed.Slice(hexPrefixSpan.Length).ToString();
                try
                {
                    if (tryParse(s, NumberStyles.HexNumber, formatProvider, out value))
                        return true;
                }
                catch (ArgumentException argExcept) when (argExcept.ParamName.Equals("style", StringComparison.OrdinalIgnoreCase)) { }
            }
            value = default;
            return false;
        }

        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value of type <typeparamref name="T"/> to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(T value) => value.ToString(format: default, formatProvider);
    }
}
