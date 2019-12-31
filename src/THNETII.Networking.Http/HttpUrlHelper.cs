using System;
using System.Collections.Generic;
using System.Text;

using THNETII.Common;

namespace THNETII.Networking.Http
{
    /// <summary>
    /// Helper class that provides useful helper methods in constructing URLs
    /// and query strings.
    /// </summary>
    public static class HttpUrlHelper
    {
        private static StringBuilder? queryBuilder;

        /// <summary>
        /// Constructs a query string using the specified enumerable of
        /// string-typed key-value-pairs. Each value is URI-escaped as URI data.
        /// </summary>
        /// <param name="queryParams">
        /// A sequence of string-typed key-value-pairs. Multiple equal key
        /// values are allowed. <see langword="null"/> values are allowed.
        /// The sequence can be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// A non-<see langword="null"/> <see cref="string"/> starting with the
        /// leading <c>'?'</c>. If <paramref name="queryParams"/> contains any
        /// pairs, the pair is added to the query string. Pairs where the key is
        /// either <see langword="null"/>, empty or contains only whitespace are
        /// skipped regardless of the value of the pair.
        /// </returns>
        public static string ToQueryString(IEnumerable<KeyValuePair<string, string?>>? queryParams)
        {
            if (queryParams is null)
                return "?";

            var queryBuilder = PrepareQueryStringBuilder();

            bool first = true;
            foreach (var queryPair in queryParams)
            {
                AppendQueryPair(queryPair, queryBuilder, ref first);
            }
            var queryString = queryBuilder.ToString();

            ResetQueryStringBuilder(queryBuilder);

            return queryString;
        }

        /// <inheritdoc cref="ToQueryString(IEnumerable{KeyValuePair{string, string?}})" />
        public static string ToQueryString(ReadOnlySpan<KeyValuePair<string, string?>> queryParams)
        {
            if (queryParams.IsEmpty)
                return "?";

            var queryBuilder = PrepareQueryStringBuilder();

            bool first = true;
            foreach (ref readonly KeyValuePair<string, string?> queryPair in queryParams)
            {
                AppendQueryPair(queryPair, queryBuilder, ref first);
            }
            var queryString = queryBuilder.ToString();

            ResetQueryStringBuilder(queryBuilder);

            return queryString;
        }

        private static StringBuilder PrepareQueryStringBuilder()
        {
            StringBuilder queryBuilder;
            (queryBuilder, HttpUrlHelper.queryBuilder) =
                (HttpUrlHelper.queryBuilder ?? new StringBuilder(), null);
            queryBuilder.Clear();

            queryBuilder.Append('?');
            return queryBuilder;
        }

        private static void AppendQueryPair(in KeyValuePair<string, string?> queryPair, StringBuilder queryBuilder, ref bool first)
        {
            if (queryPair.Key.TryNotNullOrWhiteSpace(out var key))
            {
                if (!first)
                    queryBuilder.Append('&');
                else
                    first = false;
                queryBuilder
                    .Append(key)
                    .Append('=')
                    .Append(Uri.EscapeDataString(queryPair.Value ?? string.Empty));
            }
        }

        private static void ResetQueryStringBuilder(StringBuilder queryBuilder)
        {
            if (queryBuilder.Capacity > 1024)
                queryBuilder.Capacity = 1024;
            HttpUrlHelper.queryBuilder = queryBuilder;
        }
    }
}
