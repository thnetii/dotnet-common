using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using THNETII.Common.IO;

namespace THNETII.Networking.Http.Caching
{
    public class CachingMessageHandler : DelegatingHandler
    {
        public const string CacheEntryExtension = ".resp.json";
        public const string CacheDataExtension = ".data.bin";
        private static readonly JsonSerializer defaultJsonSerializer =
            JsonSerializer.CreateDefault();

        public CachingMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        public JsonSerializer JsonSerializer { get; set; } =
            defaultJsonSerializer;

        public IFileProvider CacheProvider { get; }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual (string cacheName, byte[] cacheThumbprint)
            GetCacheName(HttpRequestMessage requ)
        {
            Uri requUri = requ.RequestUri;
            string schemeAndServer = requUri.GetComponents(
                UriComponents.Scheme | UriComponents.StrongAuthority,
                UriFormat.UriEscaped);
            string path = requUri.GetComponents(
                UriComponents.PathAndQuery,
                UriFormat.UriEscaped);

            string cacheName = GetHashString(schemeAndServer) + '/' +
                GetHashString(path);
            return (cacheName, GetHashBytes(cacheName));

            static string GetHashString(string str)
            {
                byte[] hashBytes = GetHashBytes(str);
                return string.Concat(hashBytes.Select(b => b.ToString("x2", CultureInfo.InvariantCulture)));
            }

            static byte[] GetHashBytes(string str)
            {
                byte[] urlBytes = Encoding.Unicode.GetBytes(str);
                using var hasher = SHA256.Create();
                return hasher.ComputeHash(urlBytes);
            }
        }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual async Task<object?> GetCacheEntry(byte[] cacheThumbprint,
            IFileInfo etryFileInfo, CancellationToken cancelToken = default)
        {
            if (etryFileInfo is null || !etryFileInfo.Exists)
                return null;

            byte[] etryIndicatedHash, etryContentHash;
            using var hmac = new HMACSHA256(cacheThumbprint);
            using var etryContent = new MemoryStream((int)(etryFileInfo.Length - (hmac.HashSize / 8)));
            using (var etryStream = etryFileInfo.CreateReadStream())
            {
                etryIndicatedHash = await ReadExactHashBytes(etryStream,
                    hmac.HashSize).ConfigureAwait(false);
                await etryStream.CopyToAsync(etryContent, 81920, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                etryContent.Flush();
                etryContent.Seek(0L, SeekOrigin.Begin);
            }

            etryContentHash = hmac.ComputeHash(etryContent);
            if (!etryIndicatedHash.SequenceEqual(etryContentHash))
                return null;

            etryContent.Seek(0L, SeekOrigin.Begin);
            using var etryTextReader = new StreamReader(etryContent, Encoding.UTF8);
            using var etryJsonReader = new JsonTextReader(etryTextReader) { CloseInput = false };
            return (JsonSerializer ?? defaultJsonSerializer).Deserialize<object?>(etryJsonReader);
        }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual bool CanServeFromCache(HttpRequestMessage request)
        {
            if (!request.Method.Equals(HttpMethod.Get) &&
                !request.Method.Equals(HttpMethod.Head))
                return false;

            if (request.Headers.CacheControl is { } cacheControl &&
                cacheControl.NoCache)
                return false;

            return true;
        }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual bool ShouldServeFromCache(HttpRequestMessage request,
            object cacheEntry)
        {
            return false;
        }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual void PrepareOriginRequest(HttpRequestMessage request,
            object cacheEntry)
        {

        }

        protected sealed override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancelToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            if (CanServeFromCache(request))
                return SendCacheableAsync(request, cancelToken);
            else
                return base.SendAsync(request, cancelToken);
        }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual async Task<HttpResponseMessage> SendCacheableAsync(
            HttpRequestMessage request, CancellationToken cancelToken)
        {
            var (cacheName, cacheThumb) = GetCacheName(request);
            string etryFilePath = cacheName + CacheEntryExtension;
            string dataFilePath = cacheName + CacheDataExtension;

            var etryFileInfo = CacheProvider.GetFileInfo(etryFilePath);
            var dataFileInfo = CacheProvider.GetFileInfo(dataFilePath);

            var etryInstance = await GetCacheEntry(cacheThumb, etryFileInfo)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (dataFileInfo.Exists && !(etryInstance is null) &&
                ShouldServeFromCache(request, etryInstance))
                return await GetCachedResponse(request, cacheThumb,
                    etryInstance, dataFileInfo, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);

            if (!(etryInstance is null))
                PrepareOriginRequest(request, etryInstance);

            var response = await base.SendAsync(request, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            // Re-query CacheProvider for the cache entry file
            // The physical path might have changed since the request was
            // issued.
            etryFileInfo = CacheProvider.GetFileInfo(etryFilePath);
            dataFileInfo = CacheProvider.GetFileInfo(dataFilePath);
            string? etryPhysicalPath = etryFileInfo.PhysicalPath;
            string? dataPhysicalPath = dataFileInfo.PhysicalPath;

            try
            {
                if (response.StatusCode == HttpStatusCode.NotModified)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = CreateCachedHttpContent(etryInstance, dataFileInfo);
                }
                else if (!string.IsNullOrWhiteSpace(dataPhysicalPath))
                {
                    await WriteResponseToFile(dataPhysicalPath, response, cancelToken)
                        .ConfigureAwait(continueOnCapturedContext: false);
                    response.Content = CreateCachedHttpContent(etryInstance, dataFileInfo);
                }
            }
            finally
            {

            }

            static FileStream OpenFileWriteCreateDirectory(string path)
            {
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return File.Open(path, FileMode.Create, FileAccess.Write);
            }

            static async Task WriteResponseToFile(string? filePath,
                HttpResponseMessage httpResponse,
                CancellationToken cancelToken = default)
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    return;
                using var respFile = OpenFileWriteCreateDirectory(filePath!);
                await httpResponse.Content.CopyToAsync(respFile)
                    .ConfigureAwait(continueOnCapturedContext: false);
                await respFile.FlushAsync(cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual Task<HttpResponseMessage> GetCachedResponse(
            HttpRequestMessage request, byte[] cacheThumb, object cacheEntry,
            IFileInfo dataFileInfo, CancellationToken cancelToken = default)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                ReasonPhrase = nameof(HttpStatusCode.OK),
                Content = CreateCachedHttpContent(cacheEntry, dataFileInfo),
            };

            response.Headers.Date = DateTimeOffset.UtcNow;
            response.Headers.ConnectionClose = true;

            return Task.FromResult(response);
        }

        [SuppressMessage("Design", "CA1062: Validate arguments of public methods", Justification = "Protected method")]
        protected virtual HttpContent CreateCachedHttpContent(
            object cacheEntry, IFileInfo cacheDataFile)
        {
            var content = new StreamContent(cacheDataFile.CreateReadStream());

            content.Headers.ContentLength = cacheDataFile.Length;

            return content;
        }

        private static async Task<byte[]> ReadExactHashBytes(Stream stream,
                int hashSize, CancellationToken cancelToken = default)
        {
            var hashBytes = new byte[hashSize / 8];
            int offset = 0;
            do
            {
                offset += await stream.ReadAsync(hashBytes, offset,
                    hashBytes.Length - offset, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
            } while (offset < hashBytes.Length);
            return hashBytes;
        }
    }
}
