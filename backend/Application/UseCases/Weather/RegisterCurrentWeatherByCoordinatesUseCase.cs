using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.ValueObjects.Commands;

namespace Application.UseCases.Weather;

public class RegisterCurrentWeatherByCoordinatesUseCase
{
    private readonly IWeatherProvider _weatherProvider;
    private readonly IWeatherRecordRepository _weatherRecordRepository;

    public RegisterCurrentWeatherByCoordinatesUseCase(IWeatherProvider weatherProvider, IWeatherRecordRepository weatherRecordRepository)
    {
        _weatherProvider = weatherProvider;
        _weatherRecordRepository = weatherRecordRepository;
    }

    public async Task<WeatherRecordDTO> Execute(double lat, double lon)
    {
        CoordinatesLocation location = CoordinatesLocation.Create(lat, lon);
        
        CoordinatesDTO coordinates = new CoordinatesDTO(location.Latitude, location.Longitude);
        
        WeatherDataProviderDTO weatherData = await _weatherProvider.GetWeatherDataByCoordinatesAsync(coordinates);

        Domain.Entities.WeatherRecord weatherRecord = WeatherMapper.toEntity(weatherData);
        
        await _weatherRecordRepository.AddAsync(weatherRecord);

        return WeatherMapper.fromEntityToResponseDTO(weatherRecord);
    }
}