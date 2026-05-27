using System.Net.Http.Json;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;
using Infrastructure.Exceptions;
using Infrastructure.Providers.OpenWeather.DTOs;
using Infrastructure.Providers.OpenWeather.Mappers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Providers.OpenWeather;

public class OpenWeatherProvider: IWeatherProvider, ICoordinatesProvider
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
                $"data/2.5/weather?lat={coordinates.Latitude}" +
                $"&lon={coordinates.Longitude}" +
                $"&appid={_openWeatherOptions.ApiKey}" +
                $"&units=metric");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new InfrastructureException($"No weather data found for coordinates {coordinates}.", 404);
        }
    
        response.EnsureSuccessStatusCode();
    
        OpenWeatherResponse? weatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
    
        if (weatherResponse is null)
        {
            throw new InfrastructureException("Unable to retrieve weather data.");
        }
    
        return OpenWeatherMapper.MapWeatherResponseToDto(weatherResponse);
    }

    public async Task<WeatherDataProviderDTO> GetWeatherDataByCityAsync(string city)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"data/2.5/weather?q={city}&appid={_openWeatherOptions.ApiKey}&units=metric");
    
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new InfrastructureException($"No weather data found for city {city}.", 404);
        }
    
        response.EnsureSuccessStatusCode();
    
        OpenWeatherResponse? weatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
    
        if (weatherResponse is null)
        {
            throw new InfrastructureException("Unable to retrieve weather data.");
        }
    
        return OpenWeatherMapper.MapWeatherResponseToDto(weatherResponse);
    }

    public async Task<List<CoordinatesResponseDTO>> GetCoordinatesByCityAsync(string city)
    {
     HttpResponseMessage response = await _httpClient.GetAsync(
            $"geo/1.0/direct?q={city}" +
            $"&limit=5" +
            $"&appid={_openWeatherOptions.ApiKey}");
        
        response.EnsureSuccessStatusCode();
        
        List<OpenWeatherGeoResponse>? locations = await response.Content.ReadFromJsonAsync<List<OpenWeatherGeoResponse>>();

        if (locations is null || locations.Count == 0)
        {
            throw new InfrastructureException(
                $"City '{city}' not found.",
                404);
        }
        
        return OpenWeatherMapper.MapCoordinatesResponseListToDtoList(locations);
    }
    
}