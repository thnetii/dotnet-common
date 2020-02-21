using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.Extensions.Configuration.Json.Test
{
    public static class JsonConfigurationSourceTest
    {
        [Fact]
        public static void ArraysAreMappedToConfiguration()
        {
            var jsonDict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            {
                ["Values"] = new object?[]
                {
                    "Hello",
                    "World",
                    1,
                    null,
                    false
                }
            };
            var jsonString = JsonConvert.SerializeObject(jsonDict, Formatting.Indented);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            using var jsonStream = new MemoryStream(jsonBytes);

            IConfiguration configRoot = new ConfigurationBuilder()
                .AddJsonStream(jsonStream)
                .Build();

            Assert.NotEmpty(configRoot.GetSection("Values").AsEnumerable());
        }
    }
}
