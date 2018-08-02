namespace THNETII.Networking.Http
{
    public static class HttpWellKnownMediaType
    {
        public static class Key
        {
            public const string Text = "text";
            public const string Application = "application";

            public const string Plain = "plain";
            public const string Html = "html";
        }

        public const string Html = Key.Text + "/" + Key.Html;
    }
}
