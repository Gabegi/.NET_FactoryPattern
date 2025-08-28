using FactoryApp.Domain;
using Microsoft.Extensions.Caching.Hybrid;
using System.Net;
using System.Text;

namespace FactoryApp.Infrastructure.Handlers;

public class CachingHandler : DelegatingHandler
{
    private readonly HybridCache _cache;
    private readonly WeatherServiceType _serviceType;

    public CachingHandler(HybridCache cache, WeatherServiceType serviceType)
    {
        _cache = cache;
        _serviceType = serviceType;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Only cache GET requests
        if (request.Method != HttpMethod.Get)
            return await base.SendAsync(request, cancellationToken);

        // Include query parameters and headers that might affect the response
        var cacheKey = GenerateCacheKey(request);

        var cacheResult = await _cache.GetOrCreateAsync(
            cacheKey,
            factory: async (cancellationToken) =>
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

                    return new CachedResponse
                    {
                        Content = content,
                        ContentType = contentType,
                        StatusCode = response.StatusCode,
                        IsSuccess = true
                    };
                }

                // Don't cache error responses - return null so GetOrCreateAsync doesn't cache
                return null;
            },
            options: new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30),
                // Add some jitter to prevent cache stampede
                LocalCacheExpiration = TimeSpan.FromMinutes(25)
            },
            cancellationToken: cancellationToken);

        // If we have a cached successful response, return it
        if (cacheResult?.IsSuccess == true)
        {
            return new HttpResponseMessage(cacheResult.StatusCode)
            {
                Content = new StringContent(cacheResult.Content, Encoding.UTF8, cacheResult.ContentType),
                RequestMessage = request
            };
        }

        // No cache hit or cached response was a failure - make fresh request
        return await base.SendAsync(request, cancellationToken);
    }

    private string GenerateCacheKey(HttpRequestMessage request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"weather:{_serviceType}:{request.RequestUri?.PathAndQuery}");

        // Include relevant headers that might affect the response (if any)
        // For example, Accept-Language, Authorization, etc.
        // keyBuilder.Append($":{request.Headers.AcceptLanguage?.FirstOrDefault()}");

        return keyBuilder.ToString();
    }
}

// Helper class for caching
public class CachedResponse
{
    public string Content { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/json";
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSuccess { get; set; }
}