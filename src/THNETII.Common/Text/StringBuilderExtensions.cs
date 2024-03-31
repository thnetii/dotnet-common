using System;
using System.Text;

namespace THNETII.Common.Text;

/// <summary>
/// Provides extension methods for the <see cref="StringBuilder"/> class.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Appends a span of Unicode characters to the specified
    /// <see cref="StringBuilder"/> instance.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to append to. Must not be <see langword="null"/>.</param>
    /// <param name="value">A read-only span of unicode characters to append.</param>
    /// <returns>The same instance as specified by <paramref name="builder"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Enlarging the value of <paramref name="builder"/> would exceed the
    /// <see cref="StringBuilder.MaxCapacity"/> property of <paramref name="builder"/>.
    /// </exception>
    /// <seealso href="https://github.com/dotnet/coreclr/pull/13163">Add StringBuilder Span-based APIs #13163</seealso>
    public static StringBuilder Append(this StringBuilder builder,
        ReadOnlySpan<char> value)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (value.Length > 0)
        {
            unsafe
            {
                fixed (char* valueChars = value)
                {
                    builder.Append(valueChars, value.Length);
                }
            }
        }

        return builder;
    } 
}
