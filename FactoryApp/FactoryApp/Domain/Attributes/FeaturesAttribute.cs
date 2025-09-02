namespace FactoryApp.Domain.Entities
{
    public class FeaturesAttribute : Attribute
    {
        public bool Retry { get; set; }
        public bool Caching { get; set; }
        public bool Logging { get; set; }
        public bool Auth { get; set; }
    }
}
