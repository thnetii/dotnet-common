using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace THNETII.Networking.Http
{
    public static class HttpWellKnownContentType
    {
        public static readonly MediaTypeHeaderValue Html = new System.Net.Http.Headers.MediaTypeHeaderValue(HttpWellKnownMediaType.Html);
    }
}
