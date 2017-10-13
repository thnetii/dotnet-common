namespace THNETII.Common
{
    /// <summary>
    /// Provides common extension methods for .NET types.
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// Guards a value or an instance against being <c>null</c>, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <c>null</c> are sensible.</typeparam>
        /// <param name="x">The value to check against <c>null</c>.</param>
        /// <param name="otherwise">The value to return in case <paramref name="x"/> is <c>null</c>.</param>
        /// <returns>The value of <paramref name="x"/> if <paramref name="x"/> is not <c>null</c>; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        public static T IfNotNull<T>(this T x, T otherwise) => x != null ? x : otherwise;

        /// <summary>
        /// Guards a string against being <c>null</c> or empty, returning an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c> or the empty string.</param>
        /// <param name="otherwise">The value to return in case <paramref name="s"/> is <c>null</c> or empty.</param>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <c>null</c> nor the empty string; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        public static string IfNotNullOrEmpty(this string s, string otherwise) => string.IsNullOrEmpty(s) ? otherwise : s;

        /// <summary>
        /// Guards a string against being <c>null</c>, empty or whitespace-only, returning an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c> or whitespace.</param>
        /// <param name="otherwise">The value to return in case <paramref name="s"/> is <c>null</c>, empty or whitespace-only.</param>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <c>null</c>, the empty string nor whitespace-only; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        public static string IfNotNullOrWhiteSpace(this string s, string otherwise) => string.IsNullOrWhiteSpace(s) ? otherwise : s;
    }
}
