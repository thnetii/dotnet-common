using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using THNETII.Common;

namespace THNETII.Networking.Http
{
    public static class HttpUrlHelper
    {
        private static StringBuilder queryBuilder;

        public static string ToQueryString(IEnumerable<KeyValuePair<string, string>> queryParams)
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
                if (queryPair.Key.TryNotNullOrWhiteSpace(out string key))
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

        public static unsafe string ToQueryString(ReadOnlySpan<IntPtr> queryParamKeyPtr, ReadOnlySpan<int> queryParamKeyLen, ReadOnlySpan<IntPtr> queryParamValuePtr, ReadOnlySpan<int> queryParamValueLen)
        {
            if (queryParamKeyPtr.IsEmpty)
                return "?";

            StringBuilder queryBuilder;
            (queryBuilder, HttpUrlHelper.queryBuilder) =
                (HttpUrlHelper.queryBuilder ?? new StringBuilder(), null);
            queryBuilder.Clear();

            queryBuilder.Append('?');
            bool first = true;
            for (int i = 0; i < queryParamKeyPtr.Length; i++)
            {
                char* pKey = (char*)queryParamKeyPtr[i].ToPointer();
                char* pValue = (char*)queryParamValuePtr[i].ToPointer();
                var (cKey, cValue) = (queryParamKeyLen[i], queryParamValueLen[i]);
                
                if (pKey != null && cKey > 0)
                {
                    if (!first)
                        queryBuilder.Append('&');
                    else
                        first = false;
                    queryBuilder.Append(pKey, cKey);
                    queryBuilder.Append('=');
                    if (pValue != null)
                        queryBuilder.Append(Uri.EscapeDataString(new string(pValue, 0, cValue)));
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
