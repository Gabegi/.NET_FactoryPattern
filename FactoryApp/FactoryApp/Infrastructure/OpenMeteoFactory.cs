using FactoryApp.Infrastructure.Factories;
using WeatherApp.Application.Interfaces;
using WeatherApp.Infrastructure;

namespace FactoryApp.Infrastructure
{
    public class OpenMeteoFactory : IWeatherServiceFactory
    {
        private readonly HttpClient _httpClient;

        public OpenMeteoFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IWeatherService Create()
        {
            return new OpenMeteoService(_httpClient);
        }
    }
}
