namespace FactoryApp.Application
{
    public class WeatherRequest
    {
        public string ServiceName { get; set; } = string.Empty;

        public string Environment { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public bool RequiresAuthentication { get; set; }
        public bool EnableCaching { get; set; } = true;
        public bool EnableRetryPolicy { get; set; } = true;
        public int? CustomTimeoutSeconds { get; set; }
        public string? CustomUserAgent { get; set; }
        public List<string> RequiredFeatures { get; set; } = new();
    }
}
