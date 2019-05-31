using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace THNETII.TypeConverter
{
    /// <summary>
    /// A specialized <see cref="NumberStringConverter"/> providing a specific
    /// <see cref="NumberStringConverter{T}"/> for each primitive numeric data
    /// type.
    /// </summary>
    [SuppressMessage("Naming", "CA1720: Identifier contains type name", Justification = "By design")]
    public class PrimitiveNumberStringConverter
    {
        /// <summary>
        /// Initializes and creates one <see cref="NumberStringConverter{T}"/>
        /// instance for each primitive numeric data type using the specified
        /// <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="formatProvider">
        /// An <see cref="IFormatProvider"/> that supplies culture-specific
        /// formatting information for conversion. Omit or specify <see langword="null"/>
        /// to use the <see cref="NumberFormatInfo"/> for the current culture.
        /// </param>
        public PrimitiveNumberStringConverter(IFormatProvider formatProvider = default)
        {
            Int8 = new NumberStringConverter<sbyte>(sbyte.Parse, sbyte.TryParse, formatProvider);
            UInt8 = new NumberStringConverter<byte>(byte.Parse, byte.TryParse, formatProvider);

            Int16 = new NumberStringConverter<short>(short.Parse, short.TryParse, formatProvider);
            UInt16 = new NumberStringConverter<ushort>(ushort.Parse, ushort.TryParse, formatProvider);

            Int32 = new NumberStringConverter<int>(int.Parse, int.TryParse, formatProvider);
            UInt32 = new NumberStringConverter<uint>(uint.Parse, uint.TryParse, formatProvider);

            Int64 = new NumberStringConverter<long>(long.Parse, long.TryParse, formatProvider);
            UInt64 = new NumberStringConverter<ulong>(ulong.Parse, ulong.TryParse, formatProvider);

            Single = new NumberStringConverter<float>(float.Parse, float.TryParse, formatProvider);
            Double = new NumberStringConverter<double>(double.Parse, double.TryParse, formatProvider);
            Decimal = new NumberStringConverter<decimal>(decimal.Parse, decimal.TryParse, formatProvider);
        }

        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="sbyte"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<sbyte> Int8 { get; }
        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="byte"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<byte> UInt8 { get; }

        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="short"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<short> Int16 { get; }
        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="ushort"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<ushort> UInt16 { get; }

        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="int"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<int> Int32 { get; }
        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="uint"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<uint> UInt32 { get; }

        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="long"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<long> Int64 { get; }
        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="ulong"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<ulong> UInt64 { get; }

        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="float"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<float> Single { get; }
        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="double"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<double> Double { get; }
        /// <summary>
        /// Gets a statically initialized <see cref="NumberStringConverter{T}"/>
        /// instance capable of converting to and from <see cref="decimal"/> values
        /// using the <see cref="NumberFormatInfo"/> object of the current culture.
        /// </summary>
        public NumberStringConverter<decimal> Decimal { get; }

        /// <summary>
        /// Tries to convert the specified string to a <see cref="sbyte"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out sbyte value) => Int8.TryParse(s, out value);
        /// <summary>
        /// Tries to convert the specified string to a <see cref="byte"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out byte value) => UInt8.TryParse(s, out value);

        /// <summary>
        /// Tries to convert the specified string to a <see cref="short"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out short value) => Int16.TryParse(s, out value);
        /// <summary>
        /// Tries to convert the specified string to a <see cref="ushort"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out ushort value) => UInt16.TryParse(s, out value);

        /// <summary>
        /// Tries to convert the specified string to a <see cref="int"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out int value) => Int32.TryParse(s, out value);
        /// <summary>
        /// Tries to convert the specified string to a <see cref="uint"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out uint value) => UInt32.TryParse(s, out value);

        /// <summary>
        /// Tries to convert the specified string to a <see cref="long"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out long value) => Int64.TryParse(s, out value);
        /// <summary>
        /// Tries to convert the specified string to a <see cref="ulong"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out ulong value) => UInt64.TryParse(s, out value);

        /// <summary>
        /// Tries to convert the specified string to a <see cref="float"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out float value) => Single.TryParse(s, out value);
        /// <summary>
        /// Tries to convert the specified string to a <see cref="double"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out double value) => Double.TryParse(s, out value);
        /// <summary>
        /// Tries to convert the specified string to a <see cref="decimal"/> value.
        /// The return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="value">When this method returns, if the conversion succeeded, contains the converted value; otherwise <c>0</c> (zero).</param>
        /// <returns><see langword="true"/> if value was converted successfully; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="NumberStringConverter{T}.TryParse(string, out T)"/>
        public bool TryParse(string s, out decimal value) => Decimal.TryParse(s, out value);

        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(sbyte value) => Int8.ToString(value);
        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(byte value) => UInt8.ToString(value);

        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(short value) => Int16.ToString(value);
        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(ushort value) => UInt16.ToString(value);

        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(int value) => Int32.ToString(value);
        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(uint value) => UInt32.ToString(value);

        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(long value) => Int64.ToString(value);
        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(ulong value) => UInt64.ToString(value);

        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(float value) => Single.ToString(value);
        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(double value) => Double.ToString(value);
        /// <summary>
        /// Converts the specified value to its equivalent string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The string representation of <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// The <see cref="IFormatProvider"/> specified in the <see cref="NumberStringConverter{T}(NumberStylesParseFunc{T}, NumberStylesTryParseFunc{T}, IFormatProvider)"/>
        /// constructor is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method to represent <paramref name="value"/>.
        /// </remarks>
        public string ToString(decimal value) => Decimal.ToString(value);
    }
}
