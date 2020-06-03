using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using THNETII.Common;

namespace THNETII.Networking.Http
{
    /// <summary>
    /// Provides extension methods for the <see cref="HttpContent"/> type.
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Checks whether the HTTP content represents Text content.
        /// </summary>
        /// <param name="httpContent">The HTTP Content to check. Must not be <see langword="null"/>.</param>
        /// <param name="trueIfNoContentType">
        /// An optional value controlling what value to return if <paramref name="httpContent"/> has no Content-Type information.
        /// <see langword="true"/> by default.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <c>Content-Type</c> header of <paramref name="httpContent"/> indicates text content.
        /// If <paramref name="httpContent"/> has no <c>Content-Type</c> information, <paramref name="trueIfNoContentType"/> is returned.
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <see langword="null"/>.</exception>
        /// <seealso cref="HttpContentHeaders.ContentType"/>
        public static bool IsText(this HttpContent? httpContent, bool trueIfNoContentType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsText(trueIfNoContentType);
        }

        /// <summary>
        /// Checks whether the HTTP content represents HTML content.
        /// </summary>
        /// <param name="httpContent">The HTTP Content to check. Must not be <see langword="null"/>.</param>
        /// <param name="trueIfNoContentType">
        /// An optional value controlling what value to return if <paramref name="httpContent"/> has no Content-Type information.
        /// <see langword="true"/> by default.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <c>Content-Type</c> header of <paramref name="httpContent"/> indicates HTML content.
        /// If <paramref name="httpContent"/> has no <c>Content-Type</c> information, <paramref name="trueIfNoContentType"/> is returned.
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <see langword="null"/>.</exception>
        /// <seealso cref="HttpContentHeaders.ContentType"/>
        public static bool IsHtml(this HttpContent? httpContent, bool trueIfNoContentType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsHtml(trueIfNoContentType);
        }

        /// <summary>
        /// Checks whether the HTTP content represents XML content.
        /// </summary>
        /// <param name="httpContent">The HTTP Content to check. Must not be <see langword="null"/>.</param>
        /// <param name="trueIfNoContentType">
        /// An optional value controlling what value to return if <paramref name="httpContent"/> has no Content-Type information.
        /// <see langword="true"/> by default.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <c>Content-Type</c> header of <paramref name="httpContent"/> indicates XML content.
        /// If <paramref name="httpContent"/> has no <c>Content-Type</c> information, <paramref name="trueIfNoContentType"/> is returned.
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <see langword="null"/>.</exception>
        /// <seealso cref="HttpContentHeaders.ContentType"/>
        public static bool IsXml(this HttpContent? httpContent, bool trueIfNoContentType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsXml(trueIfNoContentType);
        }

        /// <summary>
        /// Checks whether the HTTP content represents JSON content.
        /// </summary>
        /// <param name="httpContent">The HTTP Content to check. Must not be <see langword="null"/>.</param>
        /// <param name="trueIfNoContentType">
        /// An optional value controlling what value to return if <paramref name="httpContent"/> has no Content-Type information.
        /// <see langword="true"/> by default.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <c>Content-Type</c> header of <paramref name="httpContent"/> indicates JSON content.
        /// If <paramref name="httpContent"/> has no <c>Content-Type</c> information, <paramref name="trueIfNoContentType"/> is returned.
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <see langword="null"/>.</exception>
        /// <seealso cref="HttpContentHeaders.ContentType"/>
        public static bool IsJson(this HttpContent? httpContent, bool trueIfNoContentType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsJson(trueIfNoContentType);
        }

        /// <summary>
        /// Serialize the HTTP content and return an appropiate stream reader instance for reading the content as an asynchronous operation.
        /// </summary>
        /// <param name="httpContent">The HTTP content to read.</param>
        /// <param name="defaultEncoding">The default encoding to use, if none specified. May be <see langword="null"/> to guess correct encoding.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method evaluates the <see cref="HttpContentHeaders.ContentType"/> header of <paramref name="httpContent"/>
        /// and tries to create a <see cref="StreamReader"/> instance that uses the <see cref="Encoding"/> specified by
        /// the <see cref="MediaTypeHeaderValue.CharSet"/> value of the content-type.
        /// <para>
        /// Creating a <see cref="StreamReader"/> with an approiate <see cref="Encoding"/> is best effort,
        /// if the HTTP Content does not contain any charset information, a <see cref="StreamReader"/> instance is created
        /// using the default constructor.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <see langword="null"/>.</exception>
        /// <seealso cref="HttpContent.ReadAsStreamAsync"/>
        public static async Task<StreamReader> ReadAsStreamReaderAsync(this HttpContent httpContent, Encoding? defaultEncoding = null)
        {
            if (httpContent is null)
                throw new ArgumentNullException(nameof(httpContent));

            var readStreamTask = httpContent.ReadAsStreamAsync();
            Encoding? encoding = GetContentCharsetEncoding(httpContent, defaultEncoding);
            var stream = await readStreamTask.ConfigureAwait(continueOnCapturedContext: false);
            return encoding is null ? new StreamReader(stream) : new StreamReader(stream, encoding);
        }

        private static Encoding? GetContentCharsetEncoding(HttpContent httpContent, Encoding? defaultEncoding = null)
        {
            var charset = httpContent.Headers.ContentType?.CharSet;
            Encoding? encoding = defaultEncoding;
            if (!string.IsNullOrWhiteSpace(charset))
            {
                try
                {
                    encoding = Encoding.GetEncoding(charset) ?? defaultEncoding;
                }
                catch (ArgumentException) { }
            }

            return encoding;
        }
    }
}
