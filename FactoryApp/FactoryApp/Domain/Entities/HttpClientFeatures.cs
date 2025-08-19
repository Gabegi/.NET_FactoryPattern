namespace FactoryApp.Domain.Entities
{
    // 6. STRATEGY PATTERN - Feature configuration acts as strategies
    public class HttpClientFeatures
    {
        public bool EnableCaching { get; set; }
        public bool EnableAuth { get; set; }
        public string? AuthType { get; set; } // Different auth strategies: "Bearer", "ApiKey", "Basic"
        public bool EnableRetry { get; set; }
        // Each enabled feature represents a different strategy for handling requests
    }
}
