using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Configuration
{
    public class ResilienceConfigurator : IHttpClientConfigurator
    {
        public void Configure(IHttpClientBuilder clientBuilder, WeatherRequest request)
        {
            if (!request.EnableResilience) return;

            
        }
    }
}
