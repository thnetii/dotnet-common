using System;
using System.Diagnostics;
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
        /// Checks whether a passed string argument is <c>null</c>, the empty string, or only consists of white-space characters,
        /// and throws an appropiate <see cref="ArgumentException"/> if that is the case.
        /// </summary>
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
                throw value == null ? new ArgumentNullException(nameof(name)) : new ArgumentException("value must neither be empty, nor null, nor whitespace-only.", name);
            return value;
        }
    }
}
