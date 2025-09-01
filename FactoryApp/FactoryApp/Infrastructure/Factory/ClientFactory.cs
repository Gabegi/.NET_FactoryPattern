using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Factory
{
    public class ClientFactory : IClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientFactory(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Create(WeatherRequest request)
        {
            return _httpClientFactory.CreateClient(request.ClientName);
        }
    }
}