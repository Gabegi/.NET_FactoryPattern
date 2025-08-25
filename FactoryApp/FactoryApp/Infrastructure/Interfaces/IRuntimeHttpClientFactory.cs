using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IRuntimeHttpClientFactory
    {
        HttpClient Create(HttpClientFeatures features, WeatherClientCreationRequest request, WeatherServiceConfigAttribute config);
    }
}
