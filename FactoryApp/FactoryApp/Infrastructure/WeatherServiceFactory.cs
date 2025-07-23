using WeatherApp.Application.Interfaces;

namespace WeatherApp.Infrastructure;

public static class WeatherServiceFactory
{
    public static IWeatherService Create(string provider)
    {
        var httpClient = new HttpClient();

        return provider.ToLower() switch
        {
            "openmeteo" => new OpenMeteoService(httpClient),
            _ => throw new NotSupportedException($"Weather provider '{provider}' is not supported.")
        };
    }
}
