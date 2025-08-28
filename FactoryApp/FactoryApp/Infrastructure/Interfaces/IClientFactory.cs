using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IClientFactory
    {
        HttpClient Create(WeatherRequest request);
    }
}
