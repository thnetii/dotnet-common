using System;

namespace THNETII.Common
{
    public static class ArgumentExtensions
    {
        public static T ThrowIfNull<T>(this T instance, string name) where T : class
            => instance ?? throw new ArgumentNullException(name);
    }
}
