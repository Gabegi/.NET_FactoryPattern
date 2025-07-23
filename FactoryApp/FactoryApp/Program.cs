using WeatherApp.Application.Interfaces;
using WeatherApp.Infrastructure;

class Program
{
    static async Task Main()
    {
        Console.Write("Enter weather provider (e.g., openmeteo): ");
        var provider = Console.ReadLine()?.Trim() ?? "openmeteo";

        IWeatherService weatherService = WeatherServiceFactory.Create(provider);

        Console.WriteLine("Fetching weather for London (51.5, -0.1)...");

        var weather = await weatherService.GetCurrentWeatherAsync(51.5, -0.1);

        if (weather == null)
        {
            Console.WriteLine("Could not fetch weather.");
        }
        else
        {
            Console.WriteLine($"Temperature: {weather.Temperature}{weather.Units}");
            Console.WriteLine($"Wind Speed: {weather.WindSpeed} km/h");
        }
    }
}
