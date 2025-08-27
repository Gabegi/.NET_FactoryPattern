using FactoryApp.Domain;
using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;

namespace FactoryApp.Infrastructure.Configuration
{
    public class CachingConfigurator : IHttpClientConfigurator
    {
        public void Configure(
            IHttpClientBuilder clientBuilder,
            IServiceCollection services)
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

            // Add handler to HTTP pipeline
            clientBuilder.AddHttpMessageHandler(provider =>
            {
                var cache = provider.GetRequiredService<HybridCache>();

                // 👇 Pick service type based on client name
                var clientName = clientBuilder.Name;
                var serviceType = clientName switch
                {
                    "WeatherApiCached" => WeatherServiceType.OpenMeteo,
                    _ => WeatherServiceType.Unknown
                };

                return new HttpCachingHandler(cache, serviceType);
            });
        }
    }
}
