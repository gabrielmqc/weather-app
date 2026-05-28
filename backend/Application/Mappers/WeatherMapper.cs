using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Mappers;

public static class WeatherMapper
{
    public static WeatherRecordDTO FromEntityToResponseDTO(WeatherRecord weatherRecord)
    {
        return new WeatherRecordDTO(weatherRecord.Id, weatherRecord.City, weatherRecord.Temperature,
            weatherRecord.Latitude, weatherRecord.Longitude, weatherRecord.RecordedAt);
    }

    public static WeatherRecord ToEntity(WeatherDataProviderDTO weatherDataProviderDto)
    {
        return new WeatherRecord(
            weatherDataProviderDto.City, weatherDataProviderDto.Latitude, weatherDataProviderDto.Longitude,
            weatherDataProviderDto.Temperature);
    }

    public static IReadOnlyList<WeatherRecordDTO> FromEntityListToResponseDTOList(
        IReadOnlyList<WeatherRecord> weatherRecords)
    {
        return weatherRecords.Select(FromEntityToResponseDTO).ToList();
    }
}