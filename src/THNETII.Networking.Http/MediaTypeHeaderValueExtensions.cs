using System;
using System.Net.Http.Headers;
using THNETII.Common;

namespace THNETII.Networking.Http
{
    /// <summary>
    /// Provides extension methods for the <see cref="MediaTypeHeaderValue"/> type.
    /// </summary>
    public static class MediaTypeHeaderValueExtensions
    {
        /// <summary>
        /// Checks if the media type header value indicates the specified media type.
        /// </summary>
        /// <param name="header">The <see cref="MediaTypeHeaderValue"/> to check.</param>
        /// <param name="mediaType">The media type to match with the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/>.</param>
        /// <param name="trueIfNoMediaType">The value to return if <paramref name="header"/> does not indicate any media type.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> contains <paramref name="mediaType"/>.<br/>
        /// <paramref name="trueIfNoMediaType"/> if <paramref name="header"/> is <see langword="null"/> or the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> is <see langword="null"/> or only contains white-space characters.<br/>
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>String comparison is done using the <see cref="StringComparison.OrdinalIgnoreCase"/> comparison option.</remarks>
        /// <seealso cref="StringCommonExtensions.Contains(string, string, StringComparison)"/>
        public static bool ContainsMediaType(this MediaTypeHeaderValue? header, string mediaType, bool trueIfNoMediaType = true)
        {
            if (!(header?.MediaType).TryNotNullOrWhiteSpace(out var contentMediaType))
                return trueIfNoMediaType;
            return contentMediaType.Contains(mediaType, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the media type header value indicates a textual media type.
        /// </summary>
        /// <param name="header">The <see cref="MediaTypeHeaderValue"/> to check.</param>
        /// <param name="trueIfNoMediaType">The value to return if <paramref name="header"/> does not indicate any media type.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> contains <see cref="HttpWellKnownMediaType.Text"/>.<br/>
        /// <paramref name="trueIfNoMediaType"/> if <paramref name="header"/> is <see langword="null"/> or the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> is <see langword="null"/> or only contains white-space characters.<br/>
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsText(this MediaTypeHeaderValue? header, bool trueIfNoMediaType = true)
            => ContainsMediaType(header, HttpWellKnownMediaType.Text, trueIfNoMediaType);

        /// <summary>
        /// Checks if the media type header value indicates an HTML-like media type.
        /// </summary>
        /// <param name="header">The <see cref="MediaTypeHeaderValue"/> to check.</param>
        /// <param name="trueIfNoMediaType">The value to return if <paramref name="header"/> does not indicate any media type.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> contains <see cref="HttpWellKnownMediaType.Html"/>.<br/>
        /// <paramref name="trueIfNoMediaType"/> if <paramref name="header"/> is <see langword="null"/> or the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> is <see langword="null"/> or only contains white-space characters.<br/>
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsHtml(this MediaTypeHeaderValue? header, bool trueIfNoMediaType = true)
            => ContainsMediaType(header, HttpWellKnownMediaType.Html, trueIfNoMediaType);

        /// <summary>
        /// Checks if the media type header value indicates an XML-like media type.
        /// </summary>
        /// <param name="header">The <see cref="MediaTypeHeaderValue"/> to check.</param>
        /// <param name="trueIfNoMediaType">The value to return if <paramref name="header"/> does not indicate any media type.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> contains <see cref="HttpWellKnownMediaType.Json"/>.<br/>
        /// <paramref name="trueIfNoMediaType"/> if <paramref name="header"/> is <see langword="null"/> or the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> is <see langword="null"/> or only contains white-space characters.<br/>
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsJson(this MediaTypeHeaderValue? header, bool trueIfNoMediaType = true)
            => ContainsMediaType(header, HttpWellKnownMediaType.Json, trueIfNoMediaType);

        /// <summary>
        /// Checks if the media type header value indicates a JSON-like media type.
        /// </summary>
        /// <param name="header">The <see cref="MediaTypeHeaderValue"/> to check.</param>
        /// <param name="trueIfNoMediaType">The value to return if <paramref name="header"/> does not indicate any media type.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> contains <see cref="HttpWellKnownMediaType.Xml"/>.<br/>
        /// <paramref name="trueIfNoMediaType"/> if <paramref name="header"/> is <see langword="null"/> or the <see cref="MediaTypeHeaderValue.MediaType"/> property of <paramref name="header"/> is <see langword="null"/> or only contains white-space characters.<br/>
        /// Otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsXml(this MediaTypeHeaderValue? header, bool trueIfNoMediaType = true)
            => ContainsMediaType(header, HttpWellKnownMediaType.Xml, trueIfNoMediaType);
    }
}
