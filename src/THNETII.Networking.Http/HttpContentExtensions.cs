using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using THNETII.Common;

namespace THNETII.Networking.Http
{
    public static class HttpContentExtensions
    {
        public static bool IsHtml(this HttpContent httpContent, bool trueIfNoMediaType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsHtml(trueIfNoMediaType);
        }

        public static bool IsXml(this HttpContent httpContent, bool trueIfNoMediaType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsXml(trueIfNoMediaType);
        }

        public static bool IsJson(this HttpContent httpContent, bool trueIfNoMediaType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsJson(trueIfNoMediaType);
        }

        public static async Task<StreamReader> ReadAsStreamReaderAsync(this HttpContent httpContent)
        {
            var readStreamTask = httpContent.ThrowIfNull(nameof(httpContent))
                .ReadAsStreamAsync();
            var charset = httpContent.Headers.ContentType?.CharSet;
            Encoding encoding = null;
            if (!string.IsNullOrWhiteSpace(charset))
            {
                try { encoding = Encoding.GetEncoding(charset); }
                catch (ArgumentException) { }
            }
            var stream = await readStreamTask.ConfigureAwait(continueOnCapturedContext: false);
            return encoding is null ? new StreamReader(stream) : new StreamReader(stream, encoding);
        }
    }
}
