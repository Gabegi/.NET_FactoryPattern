using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Domain;
using Microsoft.Extensions.Caching.Hybrid;

namespace FactoryApp.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddWeatherHttpClients(this IServiceCollection services)
        {
            // Tokyo Dev Client
            services.AddHttpClient();
            services.AddLogging();
            services.AddHybridCache(); // Required for CachingHandler

            services.AddHttpClient("TokyoDevUser", client =>
            {
                client.BaseAddress = new Uri("https://api.open-meteo.com/v1/forecast?latitude=35.67&longitude=139.75&current_weather=true");
            })
            .AddHttpMessageHandler<LoggingHandler>();

            services.AddHttpClient("NewYorkPrdAdmin", client =>
            {
                client.BaseAddress = new Uri("https://api.open-meteo.com/v1/forecast?latitude=40.71&longitude=74&current_weather=true");
            })
            .AddHttpMessageHandler<LoggingHandler>()
            .AddHttpMessageHandler<CachingHandler>()
            .AddStandardResilienceHandler(options =>
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

            // Add handlers with proper configuration
            services.AddTransient<CachingHandler>(provider => 
                new CachingHandler(
                    provider.GetRequiredService<HybridCache>(), 
                    WeatherServiceType.TokyoDevUser));
            
            services.AddTransient<LoggingHandler>(provider => 
                new LoggingHandler(
                    provider.GetRequiredService<ILogger<LoggingHandler>>(), 
                    WeatherServiceType.TokyoDevUser));

            return services;
        }
    }
}