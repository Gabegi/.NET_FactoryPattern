namespace FactoryApp.Infrastructure.Interfaces;

public interface IConfigurableWeatherService : IWeatherClient
{
    void SetTimeout(int timeoutSeconds);
    void SetUserAgent(string userAgent);
}
