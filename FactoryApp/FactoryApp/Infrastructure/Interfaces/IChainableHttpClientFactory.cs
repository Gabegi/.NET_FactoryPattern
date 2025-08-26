using FactoryApp.Application;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IChainableHttpClientFactory
    {
        HttpClient Create(WeatherServiceConfigAttribute features, WeatherClientCreationRequest request, WeatherServiceConfigAttribute config);
    }
}
