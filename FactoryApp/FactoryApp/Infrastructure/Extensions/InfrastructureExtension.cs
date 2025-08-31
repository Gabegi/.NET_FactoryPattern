using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Infrastructure.Patterns;

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
                client.BaseAddress = new Uri("https://api.open-meteo.com/");
            })
            .AddHttpMessageHandler<LoggingHandler>();

            services.AddHttpClient("TokyoPrdAdmin", client =>
            {
                client.BaseAddress = new Uri("https://api.open-meteo.com/");
            })
            .AddHttpMessageHandler<LoggingHandler>()
            .AddHttpMessageHandler<CachingHandler>();

            // Add handlers
            services.AddTransient<CachingHandler>();
            services.AddTransient<LoggingHandler>();

            // Add core infrastructure services
            services.AddTransient<IBaseClientHandler, BaseClientHandler>(); 
            services.AddTransient<IClientFactory, ClientFactory>();

            return services;
        }
    }
}