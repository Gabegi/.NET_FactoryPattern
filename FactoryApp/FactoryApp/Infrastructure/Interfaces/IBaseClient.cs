
using FactoryApp.Application;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IBaseClient
    {
        HttpClient CreateBaseClient(WeatherClientCreationRequest request, WeatherServiceConfigAttribute config);
    }
}
