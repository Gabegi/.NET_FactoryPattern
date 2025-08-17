namespace FactoryApp.Domain.Entities
{
    public class WeatherServiceTypes
    {
        public enum WeatherServiceType
        {
            [WeatherServiceConfig("https://dev-api.openmeteo.com/v1/forecast", "asia", "dev", "Asia/Tokyo", 30, 35.6762, 139.6503)]
            TokyoDevUser,

            [WeatherServiceConfig("https://api.openmeteo.com/v1/forecast", "asia", "prod", "Asia/Tokyo", 10, 35.6762, 139.6503)]
            TokyoPrdAdmin,

            [WeatherServiceConfig("https://dev-api.openmeteo.com/v1/forecast", "europe", "dev", "Europe/London", 30, 51.5074, -0.1278)]
            LondonDevUser,

            [WeatherServiceConfig("https://api.openmeteo.com/v1/forecast", "europe", "prod", "Europe/London", 10, 51.5074, -0.1278)]
            LondonPrdAdmin,

            [WeatherServiceConfig("https://dev-api.openmeteo.com/v1/forecast", "northamerica", "dev", "America/New_York", 30, 40.7128, -74.0060)]
            NewYorkDevUser,

            [WeatherServiceConfig("https://api.openmeteo.com/v1/forecast", "northamerica", "prod", "America/New_York", 10, 40.7128, -74.0060)]
            NewYorkPrdAdmin,

            [WeatherServiceConfig("https://dev-api.openmeteo.com/v1/forecast", "global", "dev", "", 45)]
            GlobalDevUser,

            [WeatherServiceConfig("https://api.openmeteo.com/v1/forecast", "global", "prod", "", 15)]
            GlobalPrdAdmin
        }
    }
}
