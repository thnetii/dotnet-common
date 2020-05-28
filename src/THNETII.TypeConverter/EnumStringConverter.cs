using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace THNETII.TypeConverter
{
    /// <summary>
    /// Helper class that provides Enum-String Conversions.
    /// </summary>
    public static class EnumStringConverter
    {
        private delegate bool EnumTryParseFunc<T>(string s, out T value)
            where T : struct, Enum;

        private enum PlaceholderEnum : int { }

        private static class EnumValues<T> where T : struct, Enum
        {
            public static readonly Type TypeRef = typeof(T);
            public static readonly EnumTryParseFunc<T> NumericTryParse = InitializeNumericTryParse();
            public static readonly IDictionary<string, T> StringToValue = InitializeStringToValueDictionary();
            public static readonly IDictionary<T, string> ValueToString = InitializeValueToStringDictionary();

            static void InitializeConversionDictionary(Action<string, T> dictionaryAddValueAction)
            {
                var ti = TypeRef.GetTypeInfo();
                foreach (var fi in ti.DeclaredFields.Where(i => i.IsStatic))
                {
                    T v = (T)fi.GetValue(null);
                    string s = fi.Name;
                    dictionaryAddValueAction(s, v);
                }
            }

            static IDictionary<string, T> InitializeStringToValueDictionary()
            {
                var stringToValue = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
                InitializeConversionDictionary((s, v) =>
                {
                    if (s is null)
                        return;
                    if (!stringToValue.ContainsKey(s))
                        stringToValue[s] = v;
                });
                return stringToValue;
            }

            static IDictionary<T, string> InitializeValueToStringDictionary()
            {
                var valueToString = new Dictionary<T, string>();
                InitializeConversionDictionary((s, v) =>
                {
                    if (!valueToString.ContainsKey(v))
                        valueToString[v] = s;
                });
                return valueToString;
            }

            static EnumTryParseFunc<T> InitializeNumericTryParse()
            {
                var numericType = Enum.GetUnderlyingType(typeof(T));

                var miGeneric = ((Func<NumberStringConverter<int>, EnumTryParseFunc<PlaceholderEnum>>)InitializeNumericTryParseFromUnderlyingType<int, PlaceholderEnum>)
#if NETSTANDARD1_3
                    .GetMethodInfo()
#else // !NETSTANDARD1_3
                    .Method
#endif // !NETSTANDARD1_3
                    .GetGenericMethodDefinition();
                var numberConverterType = typeof(NumberStringConverter<>)
#if NETSTANDARD1_3
                    .GetTypeInfo()
#endif // NETSTANDARD1_3
                    .MakeGenericType(numericType);

                var primitiveConverter = NumberStringConverter.CurrentCulture;
                var numericConverterProperty = primitiveConverter.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(pi => pi.PropertyType == numberConverterType);
                if (numericConverterProperty is PropertyInfo)
                {
                    var numberConverter = numericConverterProperty.GetValue(primitiveConverter);
                    var miNumeric = miGeneric.MakeGenericMethod(numericType, typeof(T));
                    return (EnumTryParseFunc<T>)miNumeric.Invoke(null, new[] { numberConverter });
                }

                return null!;
            }

            static EnumTryParseFunc<TEnum> InitializeNumericTryParseFromUnderlyingType<TNumeric, TEnum>(NumberStringConverter<TNumeric> numberStringConverter)
                where TNumeric : unmanaged, IFormattable
                where TEnum : unmanaged, Enum
            {
                return TryParse;

                bool TryParse(string s, out TEnum value)
                {
                    if (numberStringConverter.TryParse(s, out var numeric))
                    {
                        unsafe
                        {
                            TEnum* pNumeric = (TEnum*)&numeric;
                            value = *pNumeric;
                        }
                    }

                    value = default;
                    return false;
                }
            }
        }

        /// <summary>
        /// Converts the string representation of the constant name, serialization name or the numeric value of one
        /// or more enumerated constants to an equivalent enumerated value of <typeparamref name="T"/>.
        /// <para>This operation is always case-insensitive using ordinal string comparison.</para>
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert to.</typeparam>
        /// <param name="s">A string containing the name, or numeric value to convert.</param>
        /// <returns>The converted value as an instance of <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static T Parse<T>(string? s) where T : struct, Enum
        {
            if (TryParse(s, out T value))
                return value;
            return (T)Enum.Parse(EnumValues<T>.TypeRef, s, ignoreCase: true);
        }

        /// <summary>
        /// Attempts to convert the string representation of the constant name, serialization name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated value of <typeparamref name="T"/>.
        /// <para>This operation is always case-insensitive using ordinal string comparison.</para>
        /// <para>Returns the default value for <typeparamref name="T"/> in case the string cannot be converted.</para>
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert to.</typeparam>
        /// <param name="s">A string containing the name, serialization name or value to convert.</param>
        /// <returns>
        /// The converted value as an instance of <typeparamref name="T"/>, or the default value of <typeparamref name="T"/> 
        /// if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static T ParseOrDefault<T>(string? s) where T : struct, Enum =>
            ParseOrDefault<T>(s, @default: default);

        /// <summary>
        /// Attempts to convert the string representation of the constant name, serialization name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated value of <typeparamref name="T"/>.
        /// <para>This operation is always case-insensitive using ordinal string comparison.</para>
        /// <para>Returns the specified alternate value in case the string cannot be converted.</para>
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert to.</typeparam>
        /// <param name="s">A string containing the name, serialization name or value to convert.</param>
        /// <param name="default">The default value to return if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>.</param>
        /// <returns>
        /// The converted value as an instance of <typeparamref name="T"/>, or <paramref name="default"/>
        /// if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static T ParseOrDefault<T>(string? s, T @default) where T : struct, Enum
        {
            if (TryParse(s, out T value))
                return value;
            return @default;
        }

        /// <summary>
        /// Attempts to convert the string representation of the constant name, serialization name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated value of <typeparamref name="T"/>.
        /// <para>This operation is always case-insensitive using ordinal string comparison.</para>
        /// <para>Returns the specified alternate value in case the string cannot be converted.</para>
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert to.</typeparam>
        /// <param name="s">A string containing the name, serialization name or value to convert.</param>
        /// <param name="defaultFactory">The factory that produces the value to return if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>. Must not be <see langword="null"/>.</param>
        /// <returns>
        /// The converted value as an instance of <typeparamref name="T"/>, or the return value from <paramref name="defaultFactory"/>
        /// if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static T ParseOrDefault<T>(string? s, Func<T> defaultFactory)
            where T : struct, Enum
        {
            if (TryParse(s, out T value))
                return value;
            else if (defaultFactory is null)
                throw new ArgumentNullException(nameof(defaultFactory));
            return defaultFactory();
        }

        /// <summary>
        /// Attempts to convert the string representation of the constant name, serialization name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated value of <typeparamref name="T"/>.
        /// <para>This operation is always case-insensitive using ordinal string comparison.</para>
        /// <para>Returns the specified alternate value in case the string cannot be converted.</para>
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert to.</typeparam>
        /// <param name="s">A string containing the name, serialization name or value to convert.</param>
        /// <param name="defaultFactory">The factory that produces the value to return if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>. Must not be <see langword="null"/>.</param>
        /// <returns>
        /// The converted value as an instance of <typeparamref name="T"/>, or the return value from <paramref name="defaultFactory"/>
        /// if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static T ParseOrDefault<T>(string? s, Func<string?, T> defaultFactory)
            where T : struct, Enum
        {
            if (TryParse(s, out T value))
                return value;
            else if (defaultFactory is null)
                throw new ArgumentNullException(nameof(defaultFactory));
            return defaultFactory(s);
        }

        /// <summary>
        /// Attempts to convert the string representation of the constant name, serialization name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated value of <typeparamref name="T"/>.
        /// <para>This operation is always case-insensitive using ordinal string comparison.</para>
        /// <para>Returns <see langword="null"/> in case the string cannot be converted.</para>
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert to.</typeparam>
        /// <param name="s">A string containing the name, serialization name or value to convert.</param>
        /// <returns>
        /// The converted value as an instance of <typeparamref name="T"/>, or <see langword="null"/>
        /// if <paramref name="s"/> cannot be converted to <typeparamref name="T"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static T? ParseOrNull<T>(string? s) where T : struct, Enum
        {
            if (TryParse(s, out T value))
                return value;
            return null;
        }

        /// <summary>
        /// Attempts to convert the string representation of the constant name, serialization name or numeric value of
        /// one or more enumerated constants to an equivalent enumerated value of <typeparamref name="T"/>.
        /// <para>This operation is always case-insensitive using ordinal string comparison.</para>
        /// <para>Returns <see langword="null"/> in case the string cannot be converted.</para>
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert to.</typeparam>
        /// <param name="s">A string containing the name, serialization name or value to convert.</param>
        /// <param name="value">The converted value of <paramref name="s"/> if the method returns <see langword="true"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="s"/> was successfully converted to a value of <typeparamref name="T"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>If this method returns <see langword="false"/>, the out-value of the <paramref name="value"/> parameter is not defined.</remarks>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static bool TryParse<T>(string? s, out T value) where T : struct, Enum
        {
            if (!(s is null))
            {
                if (EnumValues<T>.StringToValue.TryGetValue(s, out value) ||
                    EnumValues<T>.NumericTryParse(s, out value))
                    return true;
            }
            return Enum.TryParse(s, out value);
        }

        /// <summary>
        /// Returns the name or the default string representation of the specified value.
        /// </summary>
        /// <typeparam name="T">The enumeration type to convert from.</typeparam>
        /// <param name="value">The value of <typeparamref name="T"/> to serialize.</param>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static string ToString<T>(T value) where T : struct, Enum
        {
            if (EnumValues<T>.ValueToString.TryGetValue(value, out string s))
                return s;
            return value.ToString();
        }
    }
}
