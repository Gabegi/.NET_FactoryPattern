using FactoryApp.Application;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IRuntimeHttpClientFactory
    {
        HttpClient Create(WeatherServiceConfigAttribute features, WeatherRequest request, WeatherServiceConfigAttribute config);
    }
}
