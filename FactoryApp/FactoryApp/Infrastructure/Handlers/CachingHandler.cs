using FactoryApp.Application.WeatherService;
using FactoryApp.Domain;
using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;
using System.Net;
using System.Text;

namespace FactoryApp.Infrastructure.Handlers;

public class CachingHandler : IHttpClientConfigurator
{
    public void Configure(
        IHttpClientBuilder clientBuilder,
        ServiceCollection services,
        WeatherRequest request)
    {
        // Configure HybridCache
        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024 * 10; // 10MB
            options.MaximumKeyLength = 512;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30),
                LocalCacheExpiration = TimeSpan.FromMinutes(30)
            };
        });

        // Add handler to HTTP pipeline - AUTOMATIC caching
        clientBuilder.AddHttpMessageHandler(provider =>
        {
            var cache = provider.GetRequiredService<HybridCache>();
            return new HttpCachingHandler(cache, request.ServiceType);
        });
    }
}


public class HttpCachingHandler : DelegatingHandler
{
    private readonly HybridCache _cache;
    private readonly WeatherServiceType _serviceType;

    public HttpCachingHandler(HybridCache cache, WeatherServiceType serviceType)
    {
        _cache = cache;
        _serviceType = serviceType;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Method != HttpMethod.Get)
            return await base.SendAsync(request, cancellationToken);

        var cacheKey = $"weather:{_serviceType}:{request.RequestUri?.PathAndQuery}";

        // ✅ Try to get from cache first
        var cacheResult = await _cache.GetOrCreateAsync(
            cacheKey,
            factory: async (cancellationToken) =>
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    return new CachedResponse
                    {
                        Content = content,
                        StatusCode = response.StatusCode,
                        IsSuccess = true
                    };
                }
                return new CachedResponse { IsSuccess = false };
            },
            options: new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30)
            },
            cancellationToken: cancellationToken);

        if (cacheResult.IsSuccess)
        {
            return new HttpResponseMessage(cacheResult.StatusCode)
            {
                Content = new StringContent(cacheResult.Content, Encoding.UTF8, "application/json"),
                RequestMessage = request
            };
        }

        // Cache indicated failure, make fresh request
        return await base.SendAsync(request, cancellationToken);
    }
}

// Helper class for caching
public class CachedResponse
{
    public string Content { get; set; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSuccess { get; set; }
}

