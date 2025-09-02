using FactoryApp.Domain.Entities;

namespace FactoryApp.Domain
{
    public enum WeatherServiceType
    {
        [ServiceConfig(Url = "https://dev-api.openmeteo.com/v1/forecast", Region = "asia", Environment = "dev",
                       Timezone = "Tokyo", TimeoutSecond = 30, Latitude = 35.6762, Longitude = 139.6503)]
        [Features(Retry = true, Caching = true, Logging = true)]
        TokyoDevUser,

        [ServiceConfig(Url = "https://api.openmeteo.com/v1/forecast", Region = "america", Environment = "prod",
                       Timezone = "New York", TimeoutSecond = 10, Latitude = 40.71, Longitude = -74.01)]
        [Features(Retry = true, Caching = true)]
        NewYorkPrdAdmin,
    }





}
