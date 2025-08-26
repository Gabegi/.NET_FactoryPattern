using FactoryApp.Application;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IChainableHttpClientFactory
    {
        HttpClient Create(WeatherServiceConfigAttribute features, WeatherRequest request, WeatherServiceConfigAttribute config);
    }
}
