namespace FactoryApp.Domain.Entities
{
    [AttributeUsage(AttributeTargets.Field)]
    public class WeatherServiceConfigAttribute : Attribute
    {
        public string BaseUrl { get; set; }
        public string Region { get; set; }
        public string Environment { get; set; }
        public string Timezone { get; set; }
        public int TimeoutSeconds { get; set; }
        public double? DefaultLatitude { get; set; }
        public double? DefaultLongitude { get; set; }

        public WeatherServiceConfigAttribute(
            string baseUrl,
            string region,
            string environment,
            string timezone = "",
            int timeoutSeconds = 30,
            double defaultLatitude = 0,
            double defaultLongitude = 0)
        {
            BaseUrl = baseUrl;
            Region = region;
            Environment = environment;
            Timezone = timezone;
            TimeoutSeconds = timeoutSeconds;
            DefaultLatitude = defaultLatitude == 0 ? null : defaultLatitude;
            DefaultLongitude = defaultLongitude == 0 ? null : defaultLongitude;
        }
    }
}
