using System.Net;
using System.Text.Json;
using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure;
using DependencyInjectionApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace DependencyInjectionApp.Tests.Infrastructure.Services;

public class WeatherServiceTests : IDisposable
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<ILogger<OpenMeteoService>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;

    public WeatherServiceTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<OpenMeteoService>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(x => x.CreateClient("WeatherApi")).Returns(_httpClient);

        // Setup configuration mock
        _mockConfiguration.Setup(x => x["WeatherApi:BaseUrl"]).Returns("https://api.open-meteo.com/v1");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_ValidCoordinates_ReturnsWeatherData()
    {
        // Arrange
        var latitude = 52.52;
        var longitude = 13.41;
        var expectedWeather = new Weather
        {
            Latitude = latitude,
            Longitude = longitude,
            Current_weather = new CurrentWeather
            {
                Temperature = 20.5,
                Windspeed = 10.2,
                Weathercode = 0,
                Time = "2024-01-15T10:30:00",
                Interval = 900,
                Winddirection = 180,
                Is_day = 1
            }
        };

        var mockResponse = new
        {
            latitude = latitude,
            longitude = longitude,
            current_weather = new
            {
                temperature = 20.5,
                windspeed = 10.2,
                weathercode = 0,
                time = "2024-01-15T10:30:00",
                interval = 900,
                winddirection = 180,
                is_day = 1
            }
        };

        var jsonResponse = JsonSerializer.Serialize(mockResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var weatherService = new OpenMeteoService(
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);

        // Act
        var result = await weatherService.GetCurrentWeatherAsync(latitude, longitude);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedWeather.Latitude, result.Latitude);
        Assert.Equal(expectedWeather.Longitude, result.Longitude);
        Assert.Equal(expectedWeather.Current_weather.Temperature, result.Current_weather.Temperature);
        Assert.Equal(expectedWeather.Current_weather.Windspeed, result.Current_weather.Windspeed);
        Assert.Equal(expectedWeather.Current_weather.Weathercode, result.Current_weather.Weathercode);
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_HttpRequestException_ReturnsNull()
    {
        // Arrange
        var latitude = 52.52;
        var longitude = 13.41;

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var weatherService = new OpenMeteoService(
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);

        // Act
        var result = await weatherService.GetCurrentWeatherAsync(latitude, longitude);

        // Assert
        Assert.Null(result);
        VerifyLoggerWasCalled(LogLevel.Error, "Failed to fetch weather data");
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_EmptyResponse_ReturnsNull()
    {
        // Arrange
        var latitude = 52.52;
        var longitude = 13.41;

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var weatherService = new OpenMeteoService(
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);

        // Act
        var result = await weatherService.GetCurrentWeatherAsync(latitude, longitude);

        // Assert
        Assert.Null(result);
        VerifyLoggerWasCalled(LogLevel.Warning, "No weather data received");
    }

    [Theory]
    [InlineData(0, "Clear sky")]
    [InlineData(1, "Partly cloudy")]
    [InlineData(2, "Partly cloudy")]
    [InlineData(3, "Partly cloudy")]
    [InlineData(45, "Foggy")]
    [InlineData(48, "Foggy")]
    [InlineData(99, "Unknown")]
    public async Task GetCurrentWeatherAsync_DifferentWeatherCodes_ReturnsCorrectWeatherCode(
        int weatherCode,
        string expectedDescription)
    {
        // Arrange
        var latitude = 52.52;
        var longitude = 13.41;

        var mockResponse = new
        {
            latitude = latitude,
            longitude = longitude,
            current_weather = new
            {
                temperature = 20.0,
                windspeed = 5.0,
                weathercode = weatherCode,
                time = "2024-01-15T10:30:00",
                interval = 900,
                winddirection = 180,
                is_day = 1
            }
        };

        var jsonResponse = JsonSerializer.Serialize(mockResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var weatherService = new OpenMeteoService(
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);

        // Act
        var result = await weatherService.GetCurrentWeatherAsync(latitude, longitude);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(weatherCode, result.Current_weather.Weathercode);
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_CallsCorrectUrl()
    {
        // Arrange
        var latitude = 52.52;
        var longitude = 13.41;
        var expectedUrl = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var mockResponse = new
        {
            latitude = latitude,
            longitude = longitude,
            current_weather = new
            {
                temperature = 20.0,
                windspeed = 5.0,
                weathercode = 0,
                time = "2024-01-15T10:30:00",
                interval = 900,
                winddirection = 180,
                is_day = 1
            }
        };

        var jsonResponse = JsonSerializer.Serialize(mockResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        HttpRequestMessage? capturedRequest = null;
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(httpResponse);

        var weatherService = new OpenMeteoService(
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);

        // Act
        await weatherService.GetCurrentWeatherAsync(latitude, longitude);

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Equal(expectedUrl, capturedRequest.RequestUri?.ToString());
        Assert.Equal(HttpMethod.Get, capturedRequest.Method);
    }

    [Fact]
    public async Task GetCurrentWeatherAsync_LogsInformationMessage()
    {
        // Arrange
        var latitude = 52.52;
        var longitude = 13.41;

        var mockResponse = new
        {
            latitude = latitude,
            longitude = longitude,
            current_weather = new
            {
                temperature = 20.0,
                windspeed = 5.0,
                weathercode = 0,
                time = "2024-01-15T10:30:00",
                interval = 900,
                winddirection = 180,
                is_day = 1
            }
        };

        var jsonResponse = JsonSerializer.Serialize(mockResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var weatherService = new OpenMeteoService(
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);

        // Act
        await weatherService.GetCurrentWeatherAsync(latitude, longitude);

        // Assert
        VerifyLoggerWasCalled(LogLevel.Information, "Fetching weather data for coordinates");
    }

    private void VerifyLoggerWasCalled(LogLevel logLevel, string message)
    {
        _mockLogger.Verify(
            x => x.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}