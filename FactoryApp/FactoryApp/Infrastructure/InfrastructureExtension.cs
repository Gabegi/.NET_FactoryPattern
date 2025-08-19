using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using static FactoryApp.Infrastructure.Config.CachingConfig;
using Microsoft.Extensions.Caching.Hybrid;


namespace FactoryApp.Infrastructure
{
    public class InfrastructureExtension
    {
        public static class ServiceCollectionExtensions
        {
            public static IServiceCollection AddWeatherHttpClients(this IServiceCollection services)
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

                // Register the caching handler
                services.AddTransient<WeatherCachingHandler>();

                // HttpClient WITHOUT caching
                services.AddHttpClient("WeatherApi", (serviceProvider, client) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    ConfigureWeatherClient(client, configuration);
                });

                // HttpClient WITH caching
                services.AddHttpClient("WeatherApiCached", (serviceProvider, client) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    ConfigureWeatherClient(client, configuration);
                })
                .AddHttpMessageHandler<WeatherCachingHandler>(); // Add caching handler

                return services;
            }

            private static void ConfigureWeatherClient(HttpClient client, IConfiguration configuration)
            {
                var timeoutSeconds = configuration.GetValue("WeatherApi:TimeoutSeconds", 30);
                var userAgent = configuration.GetValue("WeatherApi:UserAgent", "WeatherApp/1.0");
                var baseUrl = configuration.GetValue("WeatherApi:BaseUrl", "");

                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);

                if (!string.IsNullOrEmpty(baseUrl))
                {
                    client.BaseAddress = new Uri(baseUrl);
                }
            }

            public static IServiceCollection AddWeatherServices(this IServiceCollection services)
            {
                // Register your application services
                services.AddScoped<IBaseClient, BaseClient>();
                services.AddScoped<IWeatherClientFactory, WeatherClientFactory>();
                services.AddScoped<IWeatherService, WeatherService>();

                return services;
            }
        }
    }
}
