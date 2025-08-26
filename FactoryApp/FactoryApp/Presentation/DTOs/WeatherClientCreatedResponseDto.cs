using FactoryApp.Domain;

namespace DependencyInjectionApp.Presentation.DTOs
{
    public class WeatherClientCreatedResponseDto
    {
        public string ClientId { get; set; } = string.Empty;
        public WeatherServiceType ServiceType { get; set; }
        public string BaseUrl { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public List<string> EnabledFeatures { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; } = string.Empty;
    }
}
