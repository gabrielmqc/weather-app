using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;

namespace Infrastructure.Providers.OpenWeather;

public class OpenWeatherProvider: IWeatherProvider
{
    public Task<WeatherDataProviderDTO> GetWeatherDataByCoordinatesAsync(CoordinatesDTO coordinates)
    {
        throw new NotImplementedException();
    }

    public Task<WeatherDataProviderDTO> GetWeatherDataByCityAsync(string coordinates)
    {
        throw new NotImplementedException();
    }
}