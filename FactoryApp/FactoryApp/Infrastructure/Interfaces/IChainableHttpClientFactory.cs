using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IChainableHttpClientFactory
    {
        HttpClient Create(HttpClientFeatures features, WeatherClientCreationRequest request, WeatherServiceConfigAttribute config);
    }
}
