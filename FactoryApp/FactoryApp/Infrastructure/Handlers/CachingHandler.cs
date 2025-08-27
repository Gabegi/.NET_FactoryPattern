using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;

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
    }
}

