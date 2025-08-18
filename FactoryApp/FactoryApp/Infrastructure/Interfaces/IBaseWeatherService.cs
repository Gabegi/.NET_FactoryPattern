
namespace FactoryApp.Infrastructure.Interfaces
{
    interface IBaseWeatherService
    {
        IWeatherClient CreateBaseService(string serviceName);
    }
}
