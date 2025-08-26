
using FactoryApp.Application;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IBaseClient
    {
        HttpClient CreateBaseClient(WeatherRequest request, WeatherServiceConfigAttribute config);
    }
}
