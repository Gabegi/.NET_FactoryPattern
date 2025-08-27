using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IHttpClientConfigurator
    {
        // strategy pattern
        void Configure(
            IHttpClientBuilder clientBuilder,
            IServiceCollection services);
    }
}
