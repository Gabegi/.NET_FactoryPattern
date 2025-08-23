using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IHttpClientConfigurator
    {
        void Configure(
            IHttpClientBuilder clientBuilder,
            IServiceCollection services,
            WeatherClientCreationRequest request,
            WeatherServiceConfigAttribute config);
    }
}
