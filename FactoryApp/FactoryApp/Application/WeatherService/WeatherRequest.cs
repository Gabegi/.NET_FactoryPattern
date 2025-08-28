using FactoryApp.Domain;

namespace FactoryApp.Application.WeatherService
{
    public class WeatherRequest
    {
        public string ServiceName { get; set; } = string.Empty;

        public string Environment { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public bool RequiresAuthentication { get; set; }
        public bool EnableCaching { get; set; } = true;
        public bool EnableResilience { get; set; } = true;
        public bool EnableLogging { get; set; } = true;
        public double CustomTimeoutSeconds { get; set; }
        public string? CustomUserAgent { get; set; }
        public List<string> RequiredFeatures { get; set; } = new();
        public WeatherServiceType ServiceType { get; set; }
    }
}
