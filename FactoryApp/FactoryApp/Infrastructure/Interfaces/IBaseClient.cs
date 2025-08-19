
using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces
{
    interface IBaseClient
    {
        HttpClient CreateBaseClient(WeatherClientCreationRequest request, WeatherServiceConfigAttribute config);
    }
}
