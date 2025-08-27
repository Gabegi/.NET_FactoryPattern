using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IBaseClient
    {
        IHttpClientBuilder CreateBaseClient(WeatherRequest request);
    }
}
