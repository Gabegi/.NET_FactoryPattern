using FactoryApp.Infrastructure.Configuration;
using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;


namespace FactoryApp.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
            public static IServiceCollection AddWeatherHttpClients(this IServiceCollection services)
            {

            // configs
            services.AddTransient<IHttpClientConfigurator, ResilienceConfigurator>();
            services.AddTransient<IHttpClientConfigurator, CachingConfigurator>();


            // handlers
            services.AddTransient<BaseClientHandler>();
            services.AddTransient<CachingHandler>();
            services.AddTransient<LoggingHandler>();


            return services;
            }
        }
    }

