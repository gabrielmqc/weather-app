using System.Net.Http.Json;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Providers.OpenWeather;

public class OpenWeatherProvider: IWeatherProvider
{
    
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _openWeatherOptions;
    
    public OpenWeatherProvider(
        HttpClient httpClient,
        IOptions<OpenWeatherOptions> options)
    {
        _httpClient = httpClient;
        _openWeatherOptions = options.Value;

        _httpClient.BaseAddress =
            new Uri(_openWeatherOptions.BaseUrl);
    }

    public async Task<WeatherDataProviderDTO> GetWeatherDataByCoordinatesAsync(CoordinatesDTO coordinates)
    {
        HttpResponseMessage response =
            await _httpClient.GetAsync(
                $"weather?lat={coordinates.Latitude}" +
                $"&lon={coordinates.Longitude}" +
                $"&appid={_openWeatherOptions.ApiKey}" +
                $"&units=metric");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new InfrastructureException($"No weather data found for coordinates {coordinates}.", 404);
        }
    
        response.EnsureSuccessStatusCode();
    
        OpenWeatherResponse weatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
    
        if (weatherResponse is null)
        {
            throw new InfrastructureException("Unable to retrieve weather data.");
        }
    
        return MapToDto(weatherResponse);
    }

    public async Task<WeatherDataProviderDTO> GetWeatherDataByCityAsync(string city)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"weather?q={city}&appid={_openWeatherOptions.ApiKey}&units=metric");
    
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new InfrastructureException($"No weather data found for city {city}.", 404);
        }
    
        response.EnsureSuccessStatusCode();
    
        OpenWeatherResponse weatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
    
        if (weatherResponse is null)
        {
            throw new InfrastructureException("Unable to retrieve weather data.");
        }
    
        return MapToDto(weatherResponse);
    }
    
    private static WeatherDataProviderDTO MapToDto(
        OpenWeatherResponse response)
    {
        return new WeatherDataProviderDTO(
            response.Name,
            response.Main.Temp,
            response.Coord.Lat,
            response.Coord.Lon);
    }
}