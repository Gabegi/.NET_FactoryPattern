using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Http.Resilience;


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

                services.AddHttpClient("ResilientClient")
                    .AddStandardResilienceHandler()
                    .Configure(options =>
                    {
                        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
                        options.Retry.MaxRetryAttempts = 5;
                        options.Retry.Delay = TimeSpan.Zero;
                        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(5);
                        options.CircuitBreaker.MinimumThroughput = 5;
                        options.CircuitBreaker.FailureRatio = 0.9;
                        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(5);
                        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);
                    });

                services.AddHttpClient("PlainClient");


                // Register the caching handler
                services.AddTransient<CachingHandler>();
                services.AddTransient<ResilientHandler>();
                services.AddTransient<AuthHandler>();
                services.AddTransient<CachingHandler>();

                services.AddHttpClient(); // the "raw" factory


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
                .AddHttpMessageHandler<CachingHandler>(); // Add caching handler

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
                services.AddScoped<IBaseClient, BaseClientHandler>();
                services.AddScoped<IWeatherClientFactory, WeatherClientFactory>();
                services.AddScoped<IWeatherClient, OpenMeteoClient>();

                return services;
            }
        }
    }
}
