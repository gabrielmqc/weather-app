using Application.DTOs.Response;
using Infrastructure.Providers.OpenWeather.DTOs;

namespace Infrastructure.Providers.OpenWeather.Mappers;

public class OpenWeatherMapper
{
    public static WeatherDataProviderDTO MapWeatherResponseToDto(
        OpenWeatherResponse response)
    {
        return new WeatherDataProviderDTO(
            response.Name,
            response.Main.Temp,
            response.Coord.Lat,
            response.Coord.Lon);
    }

    public static CoordinatesResponseDTO MapCoordinatesResponseToDto(OpenWeatherGeoResponse response)
    {
        return new CoordinatesResponseDTO(
            response.Name,
            response.Lat,
            response.Lon,
            response.Country,
            response.State
        );
    }

    public static List<CoordinatesResponseDTO> MapCoordinatesResponseListToDtoList(
        List<OpenWeatherGeoResponse> response)
    {
        return response.Select(x => MapCoordinatesResponseToDto(x)).ToList();
    }
}