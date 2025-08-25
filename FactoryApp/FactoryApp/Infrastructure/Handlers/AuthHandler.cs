//using System.Diagnostics.Eventing.Reader;

//namespace FactoryApp.Infrastructure.Handlers
//{
//    public class AuthHandler : DelegatingHandler
//    {
//        protected override async Task<HttpResponseMessage> SendAsync(
//        HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            // Strategy Pattern - different auth strategies based on AuthType
//            switch (_authType.ToLower())
//            {
//                case "bearer":
//                    ApplyBearerTokenStrategy(request);
//                    break;
//                case "apikey":
//                    ApplyApiKeyStrategy(request);
//                    break;
//                case "basic":
//                    ApplyBasicAuthStrategy(request);
//                    break;
//            }

//            return await base.SendAsync(request, cancellationToken);
//        }
//    }
//}
