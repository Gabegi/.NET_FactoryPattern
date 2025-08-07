namespace FactoryApp.Presentation.DTOs;

public class WeatherResponseDTO
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public CurrentWeatherDTO CurrentWeather { get; set; } = new();
}

public class CurrentWeatherDTO
{
    public string Time { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public double Windspeed { get; set; }
    public int Winddirection { get; set; }
    public int IsDay { get; set; }
    public int Weathercode { get; set; }
} 