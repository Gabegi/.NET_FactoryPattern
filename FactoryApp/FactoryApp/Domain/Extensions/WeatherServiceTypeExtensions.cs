using FactoryApp.Domain.Entities;
using System.Reflection;

namespace FactoryApp.Domain.Extensions
{
    public static class WeatherServiceTypeExtensions
    {
        public static ServiceConfigAttribute GetServiceConfig(this WeatherServiceType serviceType)
        {
            var memberInfo = typeof(WeatherServiceType).GetMember(serviceType.ToString());
            return memberInfo[0].GetCustomAttribute<ServiceConfigAttribute>()
                ?? throw new InvalidOperationException($"ServiceConfig not found for {serviceType}");
        }

        public static FeaturesAttribute GetFeatures(this WeatherServiceType serviceType)
        {
            var memberInfo = typeof(WeatherServiceType).GetMember(serviceType.ToString());
            return memberInfo[0].GetCustomAttribute<FeaturesAttribute>()
                ?? throw new InvalidOperationException($"Features not found for {serviceType}");
        }
    }
}
