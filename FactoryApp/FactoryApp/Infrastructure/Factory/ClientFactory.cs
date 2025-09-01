using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Patterns
{
    public class ClientFactory : IClientFactory
    {
        private readonly IBaseClientHandler _baseClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientBuilder _clientBuilder;


        public ClientFactory(
            IBaseClientHandler baseClient,
            IHttpClientFactory httpClientFactory,
            IHttpClientBuilder httpClientBuilder)
        {
            _baseClient = baseClient;
            _httpClientFactory = httpClientFactory;
            _clientBuilder = httpClientBuilder;
        }

        public HttpClient Create(WeatherRequest request)
        {
            return _httpClientFactory.CreateClient(request.ClientName);
        }
    }
}