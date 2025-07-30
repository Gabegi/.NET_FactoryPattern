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
        builder.Services.AddHttpClient();


        // To use mock repository instead:
        // builder.Services.AddScoped<IWeatherRepository, MockWeatherRepository>();
        builder.Services.AddScoped<IWeatherService, OpenMeteoService>();
        builder.Services.AddScoped<GetCurrentWeatherUseCase>();

        

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}