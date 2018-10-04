using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace THNETII.Common
{
    /// <summary>
    /// Provides extension methods to be used for method argument validation.
    /// </summary>
    [DebuggerStepThrough]
    public static class ArgumentExtensions
    {
        /// <summary>
        /// Checks whether the passed argument is <c>null</c>, throwing an appropiate <see cref="ArgumentNullException"/> if it is.
        /// </summary>
        /// <typeparam name="T">The type of the argument. Must be a reference type.</typeparam>
        /// <param name="instance">The instance to check for <c>null</c>.</param>
        /// <param name="name">The argument name as it appears in the calling method. Use the builtin <c>nameof</c> keyword.</param>
        /// <returns>The <paramref name="instance"/> argument, to allow for chained method calls.</returns>
        /// <remarks>
        /// This is a simple convenience extension method for a common argument <c>null</c> check for reference types.
        /// <para>
        /// As this method is implemented as an extension method, it can be called with the dot operator on <paramref name="instance"/>
        /// instance even if <paramref name="instance"/> is <c>null</c> without triggering a <see cref="NullReferenceException"/>.
        /// </para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNull<T>(this T instance, string name) where T : class
            => instance ?? throw new ArgumentNullException(name);

        /// <summary>
        /// Checks whether a passed string argument is <c>null</c> or the empty string,
        /// and throws an appropiate <see cref="ArgumentException"/> if that is the case.
        /// </summary>
        /// <param name="value">The string to check against <c>null</c> or being empty.</param>
        /// <param name="name">The argument name as it appears in the calling method. Use the builtin <c>nameof</c> keyword.</param>
        /// <returns>The value of <paramref name="value"/>, to allow for chained method calls.</returns>
        /// <remarks>
        /// In addition to calling <see cref="string.IsNullOrEmpty(string)"/>, this method performs an additional null-reference check to throw
        /// a <see cref="ArgumentNullException"/> instead of the more general <see cref="ArgumentException"/> if <paramref name="value"/> is <c>null</c>.
        /// <para>
        /// As this method is implemented as an extension method, it can be called with the dot operator on <paramref name="value"/>
        /// instance even if <paramref name="value"/> is <c>null</c> without triggering a <see cref="NullReferenceException"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ThrowIfNullOrEmpty(this string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw value is null ? new ArgumentNullException(nameof(name)) : new ArgumentException("value must neither be empty, nor null.", name);
            return value;
        }

        /// <summary>
        /// Checks whether a passed array argument is <c>null</c> or a zero-length array,
        /// and throws an appropiate <see cref="ArgumentException"/> if that is the case.
        /// </summary>
        /// <typeparam name="T">The type of the items stored in <paramref name="array"/>.</typeparam>
        /// <param name="array">The array to check against <c>null</c> or being empty.</param>
        /// <param name="name">The argument name as it appears in the calling method. Use the builtin <c>nameof</c> keyword.</param>
        /// <returns><paramref name="array"/>, to allow for chained method calls.</returns>
        /// <exception cref="ArgumentException"><paramref name="array"/> has a length of <c>0</c> (zero).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ThrowIfNullOrEmpty<T>(this T[] array, string name)
        {
            switch (array)
            {
                case null: throw new ArgumentNullException(name);
                case var _ when array.Length < 1: throw new ArgumentException($"{name} is a non-null, zero-length array.", name);
                default: return array;
            }
        }

        /// <summary>
        /// Checks whether a passed enumerable argument is <c>null</c> or empty,
        /// and throws an appropiate <see cref="ArgumentException"/> if that is the case.
        /// </summary>
        /// <typeparam name="T">The type of the elements enumerated by <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The enumerable to check against <c>null</c> or being empty.</param>
        /// <param name="name">The argument name as it appears in the calling method. Use the builtin <c>nameof</c> keyword.</param>
        /// <returns><paramref name="enumerable"/>, to allow for chained method calls.</returns>
        /// <exception cref="ArgumentException"><paramref name="enumerable"/> does not contain any elements.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> enumerable, string name)
        {
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(name);
                case var _ when !enumerable.Any(): throw new ArgumentException($"{name} is empty.", name);
                default: return enumerable;
            }
        }

        /// <summary>
        /// Checks whether a passed string argument is <c>null</c>, the empty string, or only consists of white-space characters,
        /// and throws an appropiate <see cref="ArgumentException"/> if that is the case.
        /// </summary>
        /// <param name="value">The string to check for <c>null</c>, emptiness or white-space only characters.</param>
        /// <param name="name">The argument name as it appears in the calling method. Use the builtin <c>nameof</c> keyword.</param>
        /// <returns>The value of <paramref name="value"/>, to allow for chained method calls.</returns>
        /// <remarks>
        /// In addition to calling <see cref="string.IsNullOrWhiteSpace(string)"/>, this method performs an additional null-reference check to throw
        /// a <see cref="ArgumentNullException"/> instead of the more general <see cref="ArgumentException"/> if <paramref name="value"/> is <c>null</c>.
        /// <para>
        /// As this method is implemented as an extension method, it can be called with the dot operator on <paramref name="value"/>
        /// instance even if <paramref name="value"/> is <c>null</c> without triggering a <see cref="NullReferenceException"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException"><paramref name="value"/> is either empty or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ThrowIfNullOrWhiteSpace(this string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw value is null ? new ArgumentNullException(nameof(name)) : new ArgumentException("value must neither be empty, nor null, nor whitespace-only.", name);
            return value;
        }
    }
}
