var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

// Add Swagger (optional but recommended for API testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// If you want to inject IWeatherService, you can register it like this:
// builder.Services.AddHttpClient<IWeatherService, OpenMeteoService>();

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
