// Service configuration - core weather service data
namespace FactoryApp.Domain.Entities
{
    public class ServiceConfigAttribute : Attribute
    {
        public string? Url { get; set; }
        public string? Region { get; set; }
        public string? Environment { get; set; }
        public string? Timezone { get; set; }
        public int TimeoutSecond { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}