using Microsoft.Extensions.Caching.Hybrid;
using System.Net;

namespace FactoryApp.Infrastructure.Handlers;

    public class WeatherCachingHandler : DelegatingHandler
{
    private readonly HybridCache _cache;
    private readonly ILogger<WeatherCachingHandler> _logger;

    // Configuration properties set after construction
    private string _serviceName = string.Empty;
    private int _cacheDurationMinutes = 30;

    public WeatherCachingHandler(HybridCache cache, ILogger<WeatherCachingHandler> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    // Called by the builder to configure this handler for a specific request
    public void ConfigureForRequest(string serviceName, int cacheDurationMinutes)
    {
        _serviceName = serviceName;
        _cacheDurationMinutes = cacheDurationMinutes;
        _logger.LogDebug("Configured caching handler for {ServiceName} with {Duration}min cache",
            serviceName, cacheDurationMinutes);
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Method != HttpMethod.Get)
            return await base.SendAsync(request, cancellationToken);

        // Use service-specific cache key
        var cacheKey = $"weather:{_serviceName}:{request.RequestUri}";

        var cached = await _cache.GetOrCreateAsync(cacheKey, async _ =>
        {
            _logger.LogDebug("Cache miss for {ServiceName} - {Uri}", _serviceName, request.RequestUri);
            var response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        },
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(_cacheDurationMinutes)
        });

        if (cached != null)
        {
            _logger.LogDebug("Cache hit for {ServiceName} - {Uri}", _serviceName, request.RequestUri);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(cached)
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
}
