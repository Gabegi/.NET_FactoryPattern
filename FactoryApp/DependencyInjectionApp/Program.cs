using DependencyInjectionApp.Application.UseCases;

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
        builder.Services.AddScoped<IWeatherRepository, OpenMeteoWeatherRepository>();
        builder.Services.AddScoped<GetCurrentWeatherUseCase>();

        

        var app = builder.Build();

        // Enable Swagger in Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}