using System;

namespace THNETII.Common
{
    public static class ArgumentExtensions
    {
        public static T ThrowIfNull<T>(this T instance, string name) where T : class
            => instance ?? throw new ArgumentNullException(name);

        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        public static string ThrowIfNullOrWhiteSpace(this string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw value == null ? new ArgumentNullException(nameof(name)) : new ArgumentException("value must neither be empty, nor null, nor whitespace-only.", name);
            return value;
        }
    }
}
