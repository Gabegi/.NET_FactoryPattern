using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IBaseClient
    {
        HttpClient CreateBaseClient(WeatherRequest request);
    }
}
