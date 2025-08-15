namespace AbstractFactoryApp.Infrastructure.Interfaces;

public interface IConfigurableWeatherService : IWeatherService
{
    void SetTimeout(int timeoutSeconds);
    void SetUserAgent(string userAgent);
}
