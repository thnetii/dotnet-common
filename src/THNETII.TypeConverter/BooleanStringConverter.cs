using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace THNETII.TypeConverter
{
    /// <summary>
    /// Provides conversion methods to convert strings to a boolean value.
    /// </summary>
    /// <remarks>
    /// A <see cref="string"/> value can be converted to a <see cref="bool"/> value
    /// according to the following rules (in listed order)
    /// <list type="number">
    /// <item>Using the <see cref="bool.TryParse(string, out bool)"/> method.</item>
    /// <item><see langword="null"/> is converted to <see langword="false"/>.</item>
    /// <item>
    /// Using the <see cref="int.TryParse(string, NumberStyles, IFormatProvider, out int)"/>
    /// using a <see cref="NumberStyles"/> value of <see cref="NumberStyles.Any"/>.
    /// Non-zero converted integer values convert to <see langword="true"/>,
    /// <c>0</c> (zero) converts to <see langword="false"/>.
    /// </item>
    /// <item>
    /// Using the <see cref="int.TryParse(string, NumberStyles, IFormatProvider, out int)"/>
    /// using a <see cref="NumberStyles"/> value of <see cref="NumberStyles.HexNumber"/>.
    /// The string must be prefixed with <c>"0x"</c> (case-insensitive).
    /// Non-zero converted integer values convert to <see langword="true"/>,
    /// <c>0</c> (zero) converts to <see langword="false"/>.
    /// </item>
    /// <item>
    /// If the string <c>"yes"</c> starts with the input, the input is converted
    /// to <see langword="true"/> (case-insensitive). E.g. <c>"yes"</c> and <c>"y"</c>.
    /// </item>
    /// <item>
    /// If the string <c>"no"</c> starts with the input, the input is converted
    /// to <see langword="false"/> (case-insensitive). E.g. <c>"no"</c> and <c>"n"</c>.
    /// </item>
    /// </list>
    /// All inputs may be pre- or suffixed with white-space characters.
    /// The input string is trimmed if necessary. Empty or white-space-only strings
    /// will <strong>NOT</strong> be converted to <see cref="bool"/> values.
    /// <para>
    /// <see cref="bool"/> values are converted to <see cref="string"/> values
    /// using the <see cref="bool.ToString()"/> method.
    /// </para>
    /// </remarks>
    public static class BooleanStringConverter
    {
        /// <summary>
        /// Converts the specified string to a <see cref="bool"/> value.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="s"/> contains a truthful or non-zero integer value;
        /// <see langword="false"/> if <paramref name="s"/> contains a falsy value.
        /// </returns>
        /// <remarks>
        /// See the remarks section of the <see cref="BooleanStringConverter"/>
        /// class for more information on the conversion rules.
        /// </remarks>
        /// <exception cref="FormatException"><paramref name="s"/> cannot be converted to a <see cref="bool"/> value.</exception>
        public static bool Parse(string? s)
        {
            if (bool.TryParse(s, out bool v))
                return v;
            if (TryParseAlternative(s, out v))
                return v;
            return bool.Parse(s);
        }

        /// <summary>
        /// Converts the specified string to a <see cref="bool"/> value
        /// or returns <see langword="false"/> if the string cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="s"/> contains a truthful or non-zero integer value.
        /// <see langword="false"/> if <paramref name="s"/> contains a falsy value
        /// or if <paramref name="s"/> cannot be converted to a <see cref="bool"/> value.
        /// </returns>
        /// <remarks>
        /// See the remarks section of the <see cref="BooleanStringConverter"/>
        /// class for more information on the conversion rules.
        /// </remarks>
        public static bool ParseOrDefault(string? s) => ParseOrDefault(s, default(bool));

        /// <summary>
        /// Converts the specified string to a <see cref="bool"/> value
        /// or the specified default value if the string cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="default">The value to return if <paramref name="s"/> cannot be converted to a <see cref="bool"/> value.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="s"/> contains a truthful or non-zero integer value.
        /// <see langword="false"/> if <paramref name="s"/> contains a falsy value.
        /// <paramref name="default"/> if <paramref name="s"/> cannot be converted to a <see cref="bool"/> value.
        /// </returns>
        /// <remarks>
        /// See the remarks section of the <see cref="BooleanStringConverter"/>
        /// class for more information on the conversion rules.
        /// </remarks>
        public static bool ParseOrDefault(string? s, bool @default)
            => TryParse(s, out bool value) ? value : @default;

        /// <summary>
        /// Converts the specified string to a <see cref="bool"/> value
        /// or the specified default value if the string cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="defaultFactory">The function to invoke if <paramref name="s"/> cannot be converted to a <see cref="bool"/> value. Must not be <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="s"/> contains a truthful or non-zero integer value.
        /// <see langword="false"/> if <paramref name="s"/> contains a falsy value.
        /// <paramref name="defaultFactory"/> if <paramref name="s"/> cannot be converted to a <see cref="bool"/> value.
        /// </returns>
        /// <remarks>
        /// See the remarks section of the <see cref="BooleanStringConverter"/>
        /// class for more information on the conversion rules.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public static bool ParseOrDefault(string? s, Func<bool> defaultFactory)
        {
            if (TryParse(s, out bool value))
                return value;
            if (defaultFactory is null)
                throw new ArgumentNullException(nameof(defaultFactory));
            return defaultFactory();
        }

        /// <summary>
        /// Converts the specified string to a <see cref="bool"/> value
        /// or returns <see langword="null"/> if the string cannot be converted.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="s"/> contains a truthful or non-zero integer value.
        /// <see langword="false"/> if <paramref name="s"/> contains a falsy value.
        /// <see langword="null"/> if <paramref name="s"/> cannot be converted to a <see cref="bool"/> value.
        /// </returns>
        /// <remarks>
        /// See the remarks section of the <see cref="BooleanStringConverter"/>
        /// class for more information on the conversion rules.
        /// </remarks>
        public static bool? ParseOrNull(string? s)
            => TryParse(s, out bool value) ? (bool?)value : null;

        private static bool TryParseAlternative(string? s, out bool alternateResult)
        {
            if (s is null)
            {
                alternateResult = false;
                return true;
            }
            if (s.Length < 1)
                goto parsingFailed;
            if (NumberStringConverter.CurrentCulture.TryParse(s, out int intValue))
            {
                alternateResult = intValue != 0;
                return true;
            }
            var trimmed = s.AsSpan().Trim();
            if (trimmed.Length < 1)
                goto parsingFailed;
            if ("yes".AsSpan().StartsWith(trimmed, StringComparison.OrdinalIgnoreCase))
            {
                alternateResult = true;
                return true;
            }
            else if ("no".AsSpan().StartsWith(trimmed, StringComparison.OrdinalIgnoreCase))
            {
                alternateResult = false;
                return true;
            }

        parsingFailed:
            alternateResult = default;
            return false;
        }

        /// <summary>
        /// Tries to convert the specified string to a <see cref="bool"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// See the remarks section of the <see cref="BooleanStringConverter"/>
        /// class for more information on the conversion rules.
        /// </remarks>
        public static bool TryParse(string? s, out bool value)
        {
            return (s is string && bool.TryParse(s, out value)) ||
                TryParseAlternative(s, out value);
        }

        /// <summary>
        /// Converts the specified value to its equivalent string representation (either
        /// <c>"True"</c> or <c>"False"</c>).
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to convert.</param>
        /// <returns>
        /// <see cref="bool.TrueString"/> if <paramref name="value"/> is <see langword="true"/>;
        /// otherwise, <see cref="bool.FalseString"/>.
        /// </returns>
        [SuppressMessage(category: null, "CA1305", Justification = "One-to-one wrapping around non-localized API.")]
        public static string ToString(bool value) => value.ToString();
    }
}
