using System.Net;
using System.Net.Http.Json;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Infrastructure.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Integration.Tests;

public class WeatherControllerTests : IClassFixture<WeatherAppFactory>
{
    private readonly HttpClient _client;
    private readonly WeatherAppFactory _factory;

    public WeatherControllerTests(WeatherAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_RegisterTemperatureByCityName_Debug()
    {
        var expectedWeather = new WeatherDataProviderDTO(
            "São Paulo", 
            25.5, 
            -23.5505, 
            -46.6333);
        
        _factory.WeatherProviderMock
            .Setup(x => x.GetWeatherDataByCityAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedWeather);

        var response = await _client.PostAsync("/api/weather/city?city=São Paulo", null);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task POST_RegisterTemperatureByCityName_ReturnsOk()
    {
        var expectedWeather = new WeatherDataProviderDTO(
            "São Paulo", 
            25.5, 
            -23.5505, 
            -46.6333);
        
        _factory.WeatherProviderMock
            .Setup(x => x.GetWeatherDataByCityAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedWeather);

        var response = await _client.PostAsync("/api/weather/city?city=São Paulo", null);
        var result = await response.Content.ReadFromJsonAsync<WeatherRecordDTO>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("São Paulo", result!.City);
        Assert.Equal(25.5, result.Temperature);
    }

    [Fact]
    public async Task POST_RegisterTemperatureByCoordinates_ReturnsOk()
    {
        var expectedWeather = new WeatherDataProviderDTO(
            "Rio de Janeiro", 
            30.0, 
            -22.9068, 
            -43.1729);
        
        _factory.WeatherProviderMock
            .Setup(x => x.GetWeatherDataByCoordinatesAsync(It.IsAny<CoordinatesDTO>()))
            .ReturnsAsync(expectedWeather);

        var response = await _client.PostAsync("/api/weather/coordinates?lat=-22.9068&lon=-43.1729", null);
        var result = await response.Content.ReadFromJsonAsync<WeatherRecordDTO>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Rio de Janeiro", result!.City);
        Assert.Equal(30.0, result.Temperature);
    }

    [Fact]
    public async Task GET_History_ByCity_ReturnsOk()
    {
        await ResetDatabaseAsync();

        var weatherData = new WeatherDataProviderDTO("São Paulo", 25.5, -23.5505, -46.6333);
        
        _factory.WeatherProviderMock
            .Setup(x => x.GetWeatherDataByCityAsync("São Paulo"))
            .ReturnsAsync(weatherData);

        await _client.PostAsync("/api/weather/city?city=São Paulo", null);

        var response = await _client.GetAsync("/api/weather/history?city=São Paulo");
        var result = await response.Content.ReadFromJsonAsync<List<WeatherRecordDTO>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(result!);
        Assert.Equal("São Paulo", result[0].City);
    }

    [Fact]
    public async Task GET_History_ByCoordinates_ReturnsOk()
    {
        await ResetDatabaseAsync();

        var weatherData = new WeatherDataProviderDTO("Brasília", 28.0, -15.7801, -47.9292);
        
        _factory.WeatherProviderMock
            .Setup(x => x.GetWeatherDataByCoordinatesAsync(It.IsAny<CoordinatesDTO>()))
            .ReturnsAsync(weatherData);

        await _client.PostAsync("/api/weather/coordinates?lat=-15.7801&lon=-47.9292", null);

        var response = await _client.GetAsync("/api/weather/history?lat=-15.7801&lon=-47.9292");
        var result = await response.Content.ReadFromJsonAsync<List<WeatherRecordDTO>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(result!);
        Assert.Equal("Brasília", result[0].City);
    }

    [Fact]
    public async Task GET_History_WithNoParameters_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/weather/history");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    private async Task ResetDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        db.WeatherRecords.RemoveRange(db.WeatherRecords);

        await db.SaveChangesAsync();
    }
}