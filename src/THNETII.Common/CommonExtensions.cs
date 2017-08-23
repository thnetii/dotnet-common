namespace THNETII.Common
{
    public static class CommonExtensions
    {
        public static T IfNotNull<T>(this T x, T otherwise) => x != null ? x : otherwise;
        public static string IfNotNullOrEmpty(this string s, string otherwise) => string.IsNullOrEmpty(s) ? otherwise : s;
        public static string IfNotNullOrWhiteSpace(this string s, string otherwise) => string.IsNullOrWhiteSpace(s) ? otherwise : s;
    }
}
