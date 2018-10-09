﻿using System;
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
        /// Checks whether the HTTP content represents HTML content.
        /// </summary>
        /// <param name="httpContent">The HTTP Content to check. Must not be <c>null</c>.</param>
        /// <param name="trueIfNoContentType">
        /// An optional value controlling what value to return if <paramref name="httpContent"/> has no Content-Type information.
        /// <c>true</c> by default.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <c>Content-Type</c> header of <paramref name="httpContent"/> indicates HTML content.
        /// If <paramref name="httpContent"/> has no <c>Content-Type</c> information, <paramref name="trueIfNoContentType"/> is returned.
        /// Otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <c>null</c>.</exception>
        /// <seealso cref="HttpContentHeaders.ContentType"/>
        public static bool IsHtml(this HttpContent httpContent, bool trueIfNoContentType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsHtml(trueIfNoContentType);
        }

        /// <summary>
        /// Checks whether the HTTP content represents XML content.
        /// </summary>
        /// <param name="httpContent">The HTTP Content to check. Must not be <c>null</c>.</param>
        /// <param name="trueIfNoContentType">
        /// An optional value controlling what value to return if <paramref name="httpContent"/> has no Content-Type information.
        /// <c>true</c> by default.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <c>Content-Type</c> header of <paramref name="httpContent"/> indicates XML content.
        /// If <paramref name="httpContent"/> has no <c>Content-Type</c> information, <paramref name="trueIfNoContentType"/> is returned.
        /// Otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <c>null</c>.</exception>
        /// <seealso cref="HttpContentHeaders.ContentType"/>
        public static bool IsXml(this HttpContent httpContent, bool trueIfNoContentType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsXml(trueIfNoContentType);
        }

        /// <summary>
        /// Checks whether the HTTP content represents JSON content.
        /// </summary>
        /// <param name="httpContent">The HTTP Content to check. Must not be <c>null</c>.</param>
        /// <param name="trueIfNoContentType">
        /// An optional value controlling what value to return if <paramref name="httpContent"/> has no Content-Type information.
        /// <c>true</c> by default.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <c>Content-Type</c> header of <paramref name="httpContent"/> indicates JSON content.
        /// If <paramref name="httpContent"/> has no <c>Content-Type</c> information, <paramref name="trueIfNoContentType"/> is returned.
        /// Otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <c>null</c>.</exception>
        /// <seealso cref="HttpContentHeaders.ContentType"/>
        public static bool IsJson(this HttpContent httpContent, bool trueIfNoContentType = true)
        {
            if (httpContent is null)
                return false;
            return httpContent.Headers.ContentType.IsJson(trueIfNoContentType);
        }

        /// <summary>
        /// Serialize the HTTP content and return an appropiate stream reader instance for reading the content as an asynchronous operation.
        /// </summary>
        /// <param name="httpContent">The HTTP content to read.</param>
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
        /// <exception cref="ArgumentNullException"><paramref name="httpContent"/> is <c>null</c>.</exception>
        /// <seealso cref="HttpContent.ReadAsStreamAsync"/>
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
