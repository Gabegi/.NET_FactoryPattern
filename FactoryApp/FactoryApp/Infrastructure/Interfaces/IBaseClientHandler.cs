using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IBaseClientHandler
    {
        void ConfigureBaseClient(HttpClient client, WeatherRequest request);
    }
}
