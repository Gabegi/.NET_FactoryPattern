using FactoryApp.Infrastructure.Configuration;
using FactoryApp.Infrastructure.Interfaces;


namespace FactoryApp.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
            public static IServiceCollection AddWeatherHttpClients(this IServiceCollection services)
            {

            services.AddTransient<IHttpClientConfigurator, ResilienceConfigurator>();
            services.AddTransient<IHttpClientConfigurator, CachingConfigurator>();


            return services;
            }
        }
    }

