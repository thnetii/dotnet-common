using System.Net.Http.Headers;

namespace THNETII.Networking.Http
{
    using Mime = MediaTypeHeaderValue;
    using MimeConst = HttpWellKnownMediaType;

    public static class HttpWellKnownContentType
    {
        #region Base Content Types
        public static Mime Plaintext { get; } =
            new Mime(MimeConst.TextPlain);
        public static Mime Html { get; } =
            new Mime(MimeConst.TextHtml);
        public static Mime Xml { get; } =
            new Mime(MimeConst.TextXml);
        public static Mime Json { get; } =
            new Mime(MimeConst.ApplicationJson);
        public static Mime ApplicationOctetStream { get; } =
            new Mime(MimeConst.ApplicationOctetStream);
        public static Mime Bytes { get; } =
            ApplicationOctetStream;
        #endregion
        #region UTF-8 Content Types
        public static Mime PlaintextUtf8 { get; } =
            new Mime(MimeConst.TextPlainUtf8);
        public static Mime HtmlUtf8 { get; } =
            new Mime(MimeConst.TextHtmlUtf8);
        public static Mime XmlUtf8 { get; } =
            new Mime(MimeConst.TextXmlUtf8);
        public static Mime JsonUtf8 { get; } =
            new Mime(MimeConst.ApplicationJsonUtf8);
        #endregion
    }
}
