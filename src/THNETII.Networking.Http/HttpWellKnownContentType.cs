using System.Net.Http.Headers;

namespace THNETII.Networking.Http
{
    using Mime = MediaTypeHeaderValue;
    using MimeConst = HttpWellKnownMediaType;

    public static class HttpWellKnownContentType
    {
        public static Mime Plaintext { get; }
            = new Mime(MimeConst.TextPlain);
        public static Mime Html { get; }
            = new Mime(MimeConst.TextHtml);
        public static Mime Xml { get; }
            = new Mime(MimeConst.TextXml);
        public static Mime Json { get; }
            = new Mime(MimeConst.ApplicationJson);
        public static Mime ApplicationOctetStream { get; }
            = new Mime(MimeConst.ApplicationOctetStream);
        public static Mime Bytes { get; }
            = ApplicationOctetStream;
    }
}
