namespace DependencyInjectionApp.Controllers.DTOs
{
    public class WeatherInfo
    {
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public string Units { get; set; } = "°C";
    }
} 