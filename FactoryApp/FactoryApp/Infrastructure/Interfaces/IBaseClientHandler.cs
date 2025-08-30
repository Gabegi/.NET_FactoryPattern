using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IBaseClientHandler
    {
        IHttpClientBuilder CreateBaseClient(WeatherRequest request);
    }
}
