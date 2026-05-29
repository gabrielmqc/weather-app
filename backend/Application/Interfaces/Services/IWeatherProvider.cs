using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;

public interface IWeatherProvider
{
    Task<WeatherDataProviderDTO> GetWeatherDataByCoordinatesAsync(CoordinatesDTO coordinates);
    Task<WeatherDataProviderDTO> GetWeatherDataByCityAsync(string city);
}