using FactoryApp.Domain.Entities;

namespace FactoryApp.Domain
{
    public enum WeatherServiceType
    {
        [ServiceConfig(Url = "https://dev-api.openmeteo.com/v1/forecast", Region = "asia", Environment = "dev",
                       Timezone = "Tokyo", TimeoutSecond = 30, Latitude = 35.6762, Longitude = 139.6503)]
        [Features(Retry = true, Caching = true, Logging = true)]
        TokyoDevUser,

        [ServiceConfig(Url = "https://api.openmeteo.com/v1/forecast", Region = "asia", Environment = "prod",
                       Timezone = "Tokyo", TimeoutSecond = 10, Latitude = 35.6762, Longitude = 139.6503)]
        [Features(Retry = true, Caching = true)]
        TokyoPrdAdmin,
    }





}
