using System.Net;
using System.Text.Json;
using DependencyInjectionApp.Domain.Entities;
using Moq;
using Moq.Protected;
using WeatherApp.Infrastructure;
using Xunit;

namespace DependencyInjectionApp.Tests.Infrastructure.Services;

public class WeatherServiceTests
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
            Temperature = 20.5,
            Humidity = 65.0,
            WindSpeed = 10.2,
            Description = "Clear sky"
        };

        var mockResponse = new
        {
            current_weather = new
            {
                temperature = 20.5,
                relative_humidity_2m = 65.0,
                wind_speed_10m = 10.2,
                weather_code = 0
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
        Assert.Equal(expectedWeather.Temperature, result.Temperature);
        Assert.Equal(expectedWeather.Humidity, result.Humidity);
        Assert.Equal(expectedWeather.WindSpeed, result.WindSpeed);
        Assert.Equal(expectedWeather.Description, result.Description);
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

        var weatherService = new OpenMeteoWeatherService(
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

        var weatherService = new OpenMeteoWeatherService(
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
    public async Task GetCurrentWeatherAsync_DifferentWeatherCodes_ReturnsCorrectDescription(
        int weatherCode,
        string expectedDescription)
    {
        // Arrange
        var latitude = 52.52;
        var longitude = 13.41;

        var mockResponse = new
        {
            current_weather = new
            {
                temperature = 20.0,
                relative_humidity_2m = 60.0,
                wind_speed_10m = 5.0,
                weather_code = weatherCode
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

        var weatherService = new OpenMeteoWeatherService(
            _mockHttpClientFactory.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);

        // Act
        var result = await weatherService.GetCurrentWeatherAsync(latitude, longitude);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDescription, result.Description);
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
            current_weather = new
            {
                temperature = 20.0,
                relative_humidity_2m = 60.0,
                wind_speed_10m = 5.0,
                weather_code = 0
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

        var weatherService = new OpenMeteoWeatherService(
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
            current_weather = new
            {
                temperature = 20.0,
                relative_humidity_2m = 60.0,
                wind_speed_10m = 5.0,
                weather_code = 0
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

        var weatherService = new OpenMeteoWeatherService(
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

>