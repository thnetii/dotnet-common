namespace THNETII.Networking.Http
{
    public static class HttpWellKnownMediaType
    {
        #region Prefixes
        public const string Text = "text";
        public const string Application = "application";
        #endregion
        #region Suffixes
        public const string Plain = "plain";
        public const string Html = "html";
        public const string Json = "json";
        public const string Xml = "xml";
        private const string OctetStream = "octet-stream";
        #endregion
        #region Full Media Types
        public const string TextPlain = Text + "/" + Plain;
        public const string TextHtml = Text + "/" + Html;
        public const string TextXml = Text + "/" + Xml;

        public const string ApplicationJson = Application + "/" + Json;
        public const string ApplicationOctetStream = Application + "/" + OctetStream;
        #endregion
    }
}
