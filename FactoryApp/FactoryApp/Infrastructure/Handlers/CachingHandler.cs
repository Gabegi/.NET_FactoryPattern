using Microsoft.Extensions.Caching.Hybrid;
using System.Net;

namespace FactoryApp.Infrastructure.Handlers;

    public class CachingHandler : DelegatingHandler
{
    private readonly HybridCache _cache;
    private readonly ILogger<CachingHandler> _logger;

    private string _serviceName = string.Empty;
    private int _cacheDurationMinutes = 30;

    public CachingHandler(HybridCache cache, ILogger<CachingHandler> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Method != HttpMethod.Get)
            return await base.SendAsync(request, cancellationToken);

        // Use service-specific cache key
        var cacheKey = $"http:{_serviceName}:{request.RequestUri}";

        var cached = await _cache.GetOrCreateAsync(
            cacheKey,
            async token =>
            {
                _logger.LogDebug("Cache miss for {ServiceName} - {Uri}", _serviceName, request.RequestUri);

                var response = await base.SendAsync(request, token);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync(token);
                    var contentType = response.Content.Headers.ContentType?.ToString();

                    // Store both content and content type for proper reconstruction
                    var cacheData = new CachedResponse
                    {
                        Content = content,
                        ContentType = contentType,
                        StatusCode = response.StatusCode
                    };

                    response.Dispose();
                    return cacheData;
                }

                response.Dispose();
                return null; // Don't cache failures
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(_cacheDurationMinutes)
            });

        if (cached != null)
        {
            _logger.LogDebug("Cache hit for {ServiceName} - {Uri}", _serviceName, request.RequestUri);

            var response = new HttpResponseMessage(cached.StatusCode)
            {
                Content = new ByteArrayContent(cached.Content),
                RequestMessage = request
            };

            // Restore content type if it was cached
            if (!string.IsNullOrEmpty(cached.ContentType))
            {
                response.Content.Headers.TryAddWithoutValidation("Content-Type", cached.ContentType);
            }

            return response;
        }

        // If cache returned null (failure case), make fresh request
        return await base.SendAsync(request, cancellationToken);
    }

    public class CachedResponse
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string? ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }
}


