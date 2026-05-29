using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.ValueObjects.Commands;

namespace Application.UseCases.Weather;

public class RegisterCurrentWeatherByCoordinatesUseCase
{
    private readonly IWeatherProviderFactory _weatherProviderFactory;
    private readonly IWeatherRecordRepository _weatherRecordRepository;

    public RegisterCurrentWeatherByCoordinatesUseCase(IWeatherProviderFactory weatherProviderFactory, IWeatherRecordRepository weatherRecordRepository)
    {
        _weatherProviderFactory = weatherProviderFactory;
        _weatherRecordRepository = weatherRecordRepository;
    }

    public async Task<WeatherRecordDTO> Execute(double lat, double lon)
    {
        CoordinatesLocation location = CoordinatesLocation.Create(lat, lon);
        
        CoordinatesDTO coordinates = new CoordinatesDTO(location.Latitude, location.Longitude);
        
        var weatherProvider = _weatherProviderFactory.GetProvider();

        WeatherDataProviderDTO weatherData = await weatherProvider.GetWeatherDataByCoordinatesAsync(coordinates);

        Domain.Entities.WeatherRecord weatherRecord = WeatherMapper.ToEntity(weatherData);
        
        await _weatherRecordRepository.AddAsync(weatherRecord);

        return WeatherMapper.FromEntityToResponseDTO(weatherRecord);
    }
}