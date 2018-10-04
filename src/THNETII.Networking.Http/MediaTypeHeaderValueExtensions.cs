using System;
using System.Net.Http.Headers;
using THNETII.Common;

namespace THNETII.Networking.Http
{
    public static class MediaTypeHeaderValueExtensions
    {
        public static bool ContainsMediaType(this MediaTypeHeaderValue contentType, string mediaType, bool trueIfNoMediaType = true)
        {
            if (contentType is null)
                return trueIfNoMediaType;
            else if (string.IsNullOrWhiteSpace(contentType.MediaType))
                return trueIfNoMediaType;
            return contentType.MediaType.Contains(mediaType, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsHtml(this MediaTypeHeaderValue contentType, bool trueIfNoMediaType = true)
            => ContainsMediaType(contentType, HttpWellKnownMediaType.Html, trueIfNoMediaType);

        public static bool IsJson(this MediaTypeHeaderValue contentType, bool trueIfNoMediaType = true)
            => ContainsMediaType(contentType, HttpWellKnownMediaType.Json, trueIfNoMediaType);

        public static bool IsXml(this MediaTypeHeaderValue contentType, bool trueIfNoMediaType = true)
            => ContainsMediaType(contentType, HttpWellKnownMediaType.Xml, trueIfNoMediaType);
    }
}
