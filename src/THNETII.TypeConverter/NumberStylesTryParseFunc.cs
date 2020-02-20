using System;
using System.Globalization;

namespace THNETII.TypeConverter
{
    using static NumberStyles;

    /// <summary>
    /// Represents a function that converts a string representation of a number
    /// in a specified style and culture-specific format to its equivalent value
    /// of type <typeparamref name="T"/>. A return value indicated whether the
    /// conversion succeeded.
    /// </summary>
    /// <typeparam name="T">The type to which the string representation is converted.</typeparam>
    /// <param name="s">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="style"/>.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="s"/>. A typical value to specify is <see cref="Integer"/>.</param>
    /// <param name="formatProvider">An object that supplies culture-specific information about the format of <paramref name="s"/>.</param>
    /// <param name="result">
    /// When this method returns, contains the value of type <typeparamref name="T"/>
    /// that is equivalent to the number contained in <paramref name="s"/>, if
    /// the conversion succeeded, or the default value of <typeparamref name="T"/>
    /// if the conversion failed. This parameter is passed uninitialized; any
    /// value originally supplied in result will be overwritten.
    /// </param>
    /// <returns><see langword="true"/> is <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="style"/> is not a valid combination of the <see cref="NumberStyles"/> enumeration.</exception>
    public delegate bool NumberStylesTryParseFunc<T>(string? s, NumberStyles style,
        IFormatProvider? formatProvider, out T result);
}
