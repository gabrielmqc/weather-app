using System.Net.Http.Json;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;
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
        var response =
            await _httpClient.GetFromJsonAsync<OpenWeatherResponse>(
                $"weather?lat={coordinates.Latitude}" +
                $"&lon={coordinates.Longitude}" +
                $"&appid={_openWeatherOptions.ApiKey}" +
                $"&units=metric");

        if (response is null)
        {
            throw new Exception(
                "Unable to retrieve weather data.");
        }

        return MapToDto(response);
    }

    public async Task<WeatherDataProviderDTO> GetWeatherDataByCityAsync(string city)
    {
        var response =
            await _httpClient.GetFromJsonAsync<OpenWeatherResponse>(
                $"weather?q={city}" +
                $"&appid={_openWeatherOptions.ApiKey}" +
                $"&units=metric");

        if (response is null)
        {
            throw new Exception(
                "Unable to retrieve weather data.");
        }

        return MapToDto(response);
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