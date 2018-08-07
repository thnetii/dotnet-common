using System.Net.Http;

namespace THNETII.Networking.Http
{
    public static class HttpContentExtensions
    {
        public static bool IsHtml(this HttpContent httpContent, bool trueIfNoMediaType = true)
        {
            if (httpContent == null)
                return false;
            return httpContent.Headers.ContentType.IsHtml(trueIfNoMediaType);
        }

        public static bool IsXml(this HttpContent httpContent, bool trueIfNoMediaType = true)
        {
            if (httpContent == null)
                return false;
            return httpContent.Headers.ContentType.IsXml(trueIfNoMediaType);
        }

        public static bool IsJson(this HttpContent httpContent, bool trueIfNoMediaType = true)
        {
            if (httpContent == null)
                return false;
            return httpContent.Headers.ContentType.IsJson(trueIfNoMediaType);
        }
    }
}
