namespace FactoryApp.Domain.Entities
{

        public enum WeatherServiceType
        {
            [HttpClientFeatures("https://dev-api.openmeteo.com/v1/forecast", "asia", "dev", "Tokyo", 30, 35.6762, 139.6503)]
            TokyoDevUser,

            [HttpClientFeatures("https://api.openmeteo.com/v1/forecast", "asia", "prod", "Tokyo", 10, 35.6762, 139.6503)]
            TokyoPrdAdmin,

            [HttpClientFeatures("https://dev-api.openmeteo.com/v1/forecast", "europe", "dev", "London", 30, 51.5074, -0.1278)]
            LondonDevUser,

            [HttpClientFeatures("https://api.openmeteo.com/v1/forecast", "europe", "prod", "Europe/London", 10, 51.5074, -0.1278)]
            LondonPrdAdmin,

            [HttpClientFeatures("https://dev-api.openmeteo.com/v1/forecast", "northamerica", "dev", "America/New_York", 30, 40.7128, -74.0060)]
            NewYorkDevUser,

            [HttpClientFeatures("https://api.openmeteo.com/v1/forecast", "northamerica", "prod", "America/New_York", 10, 40.7128, -74.0060)]
            NewYorkPrdAdmin,

            [HttpClientFeatures("https://dev-api.openmeteo.com/v1/forecast", "global", "dev", "", 45)]
            GlobalDevUser,

            [HttpClientFeatures("https://api.openmeteo.com/v1/forecast", "global", "prod", "", 15)]
            GlobalPrdAdmin
        }




    
}
