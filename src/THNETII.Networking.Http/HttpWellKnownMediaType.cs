namespace THNETII.Networking.Http
{
    /// <summary>
    /// Provides well-known HTTP Media Type string constants.
    /// </summary>
    public static class HttpWellKnownMediaType
    {
        #region Prefixes

        /// <summary>
        /// The MIME Type for any document that contains text and is theoretically human readable.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Text = "text";

        /// <summary>
        /// The MIME Type for any kind of images. Videos are not included, though animated images (like animated gif) are described with an image type.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Image = "image";

        /// <summary>
        /// The MIME Type for any kind of audio files.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Audio = "audio";

        /// <summary>
        /// The MIME Type for any kind of video files.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Video = "video";

        /// <summary>
        /// The MIME Type for the category of document that are broken in distinct parts, often with different MIME types.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Multipart = "multipart";

        /// <summary>
        /// Gets the MIME Type for binary data content.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Application = "application";

        #endregion
        #region Suffixes

        /// <summary>
        /// The plain subtype for the <see cref="Text"/> type.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Plain = "plain";

        /// <summary>
        /// The subtype indicating HTML content.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Html = "html";

        /// <summary>
        /// The subtype indicating JSON content.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Json = "json";

        /// <summary>
        /// The subtype indicating XML content.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string Xml = "xml";

        /// <summary>
        /// The subtype indicating any binary data.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        private const string OctetStream = "octet-stream";

        #endregion
        #region Parameter Suffixes

        /// <summary>
        /// The parameter suffix indicating text using <see cref="System.Text.Encoding.UTF8"/> text encoding.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string CharSetUtf8Parameter = "; charset=utf-8";

        #endregion
        #region Dicrete Media Types

        /// <summary>
        /// The discrete media type string for unknown textual data.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string TextPlain = Text + "/" + Plain;

        /// <summary>
        /// The dicrete media type string for HTML text content.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string TextHtml = Text + "/" + Html;

        /// <summary>
        /// The dicrete media type string for XML text content.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string TextXml = Text + "/" + Xml;

        /// <summary>
        /// The discrete media type string for JSON content.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string ApplicationJson = Application + "/" + Json;

        /// <summary>
        /// The discrete media type string for arbitrary binary data.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string ApplicationOctetStream = Application + "/" + OctetStream;

        #endregion
        #region UTF-8 Full Media Types

        /// <summary>
        /// The discrete media type string for plaintext content, with the UTF-8 text encoding parameter applied.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string TextPlainUtf8 = TextPlain + CharSetUtf8Parameter;

        /// <summary>
        /// The discrete media type string for HTML content, with the UTF-8 text encoding parameter applied.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string TextHtmlUtf8 = TextHtml + CharSetUtf8Parameter;

        /// <summary>
        /// The discrete media type string for XML content, with the UTF-8 text encoding parameter applied.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string TextXmlUtf8 = TextXml + CharSetUtf8Parameter;

        /// <summary>
        /// The discrete media type string for JSON content, with the UTF-8 text encoding parameter applied.
        /// </summary>
        /// <value>A compile-time constant <see cref="string"/> literal.</value>
        public const string ApplicationJsonUtf8 = ApplicationJson + CharSetUtf8Parameter;
        #endregion
    }
}
