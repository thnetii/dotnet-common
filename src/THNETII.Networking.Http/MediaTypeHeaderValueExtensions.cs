using System;
using System.Net.Http.Headers;
using THNETII.Common;

namespace THNETII.Networking.Http
{
    public static class MediaTypeHeaderValueExtensions
    {
        public static bool IsHtml(this MediaTypeHeaderValue contentType, bool trueIfNoMediaType = true)
        {
            if (contentType == null)
                return trueIfNoMediaType;
            else if (string.IsNullOrWhiteSpace(contentType.MediaType))
                return trueIfNoMediaType;
            return contentType.MediaType.Contains(
                HttpWellKnownMediaType.Key.Html,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
