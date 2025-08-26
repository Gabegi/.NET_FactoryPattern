using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IChainableHttpClientFactory
    {
        HttpClient Create(WeatherRequest request);
    }
}
