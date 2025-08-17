
namespace FactoryApp.Infrastructure.Interfaces
{
    interface IBaseWeatherService
    {
        IWeatherService CreateBaseService(string serviceName);
    }
}
