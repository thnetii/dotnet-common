using System;
using System.Globalization;

namespace THNETII.TypeConverter
{
    using static NumberStyles;

    /// <summary>
    /// Represents a function that parses a string value into a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s"/>. A typical value to specify is <see cref="Integer"/>.</param>
    /// <param name="formatProvider">An object that supplies culture-specific information about the format of <paramref name="s"/>.</param>
    /// <returns>A value of type <typeparamref name="T"/> that is equivalent to the string specified in <paramref name="s"/>.</returns>
    /// <seealso cref="int.Parse(string, NumberStyles, IFormatProvider)"/>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="style"/> is not a valid combination of the <see cref="NumberStyles"/> enumeration.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with <paramref name="style"/>.</exception>
    /// <exception cref="OverflowException">The number represented by <paramref name="s"/> cannot losslessly fit into a value of type <typeparamref name="T"/>.</exception>
    public delegate T NumberStylesParseFunc<T>(string s, NumberStyles style,
        IFormatProvider? formatProvider);
}
