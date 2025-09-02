using FactoryApp.Domain;

namespace FactoryApp.Application.WeatherService
{
    public class WeatherRequest
    {
        public string ClientName { get; set; } = string.Empty;
        public WeatherServiceType ServiceType { get; set; }
    }
}
