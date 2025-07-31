using DependencyInjectionApp.Application.UseCases;
using DependencyInjectionApp.Infrastructure;
using DependencyInjectionApp.Infrastructure.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        // Register HttpClient with configuration from appsettings
        builder.Services.AddHttpClient("WeatherApi", (serviceProvider, client) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var timeoutSeconds = configuration.GetValue<int>("WeatherApi:TimeoutSeconds", 30);
            var userAgent = configuration.GetValue<string>("WeatherApi:UserAgent", "WeatherApp/1.0");

            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        });


        // To use mock repository instead:
        builder.Services.AddScoped<IWeatherService, MockWeatherService>();
        builder.Services.AddScoped<IWeatherService, OpenMeteoService>();
        builder.Services.AddScoped<GetCurrentWeatherUseCase>();
        builder.Services.AddLogging();



        var app = builder.Build();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}