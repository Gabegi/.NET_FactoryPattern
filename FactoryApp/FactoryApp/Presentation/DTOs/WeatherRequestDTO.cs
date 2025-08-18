
namespace FactoryApp.Presentation.DTOs
{
    public class WeatherRequestDTO
    {
        public required string ServiceName { get; init; } 
        public string Environment { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
    }
}
