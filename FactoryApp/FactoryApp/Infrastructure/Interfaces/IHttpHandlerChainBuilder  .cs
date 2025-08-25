namespace FactoryApp.Infrastructure.Interfaces
{
    public interface IHttpHandlerChainBuilder
    {
        HttpClient BuildClientWithHandlers(HttpClient baseClient, /*HttpClientFeatures features, */string serviceName);

    }
}
