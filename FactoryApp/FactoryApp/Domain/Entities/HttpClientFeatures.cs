namespace FactoryApp.Domain.Entities
{
    // 6. STRATEGY PATTERN - Feature configuration acts as strategies
    public class HttpClientFeatures
    {
        public string Url { get; set; }
        public string Region { get; set; }

        public string Environment { get; set; }
        public string Timezone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TimeoutSecond { get; set; }
  

        public bool EnableLogging { get; set; }
        public bool EnableCaching { get; set; }
        public bool EnableAuth { get; set; }
        public string? AuthType { get; set; } // Different auth strategies: "Bearer", "ApiKey", "Basic"
        public bool EnableRetry { get; set; }
        // Each enabled feature represents a different strategy for handling requests
    }
}
