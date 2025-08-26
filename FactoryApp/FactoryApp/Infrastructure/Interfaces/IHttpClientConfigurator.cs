using FactoryApp.Application;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IHttpClientConfigurator
    {
        void Configure(
            IHttpClientBuilder clientBuilder,
            IServiceCollection services,
            WeatherRequest request,
            WeatherServiceConfigAttribute config);
    }
}
