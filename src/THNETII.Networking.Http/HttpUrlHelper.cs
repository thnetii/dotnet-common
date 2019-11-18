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

            StringBuilder queryBuilder;
            (queryBuilder, HttpUrlHelper.queryBuilder) =
                (HttpUrlHelper.queryBuilder ?? new StringBuilder(), null);
            queryBuilder.Clear();

            queryBuilder.Append('?');
            bool first = true;
            foreach (var queryPair in queryParams)
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
            var queryString = queryBuilder.ToString();

            if (queryBuilder.Capacity > 1024)
                queryBuilder.Capacity = 1024;
            HttpUrlHelper.queryBuilder = queryBuilder;

            return queryString;
        }
    }
}
