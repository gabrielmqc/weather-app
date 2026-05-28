using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.ValueObjects.Commands;

namespace Application.UseCases.Weather;

public class RegisterCurrentWeatherByCityNameUseCase
{
    private readonly IWeatherProvider _weatherProvider;
    private readonly IWeatherRecordRepository _weatherRecordRepository;

    public RegisterCurrentWeatherByCityNameUseCase(IWeatherProvider weatherProvider, IWeatherRecordRepository weatherRecordRepository)
    {
        _weatherProvider = weatherProvider;
        _weatherRecordRepository = weatherRecordRepository;
    }

    public async Task<WeatherRecordDTO> Execute(string cityName)
    {
        CityLocation location = CityLocation.Create(cityName);

        WeatherDataProviderDTO weatherData = await _weatherProvider.GetWeatherDataByCityAsync(location.Name);

        Domain.Entities.WeatherRecord weatherRecord = WeatherMapper.ToEntity(weatherData);
        
        await _weatherRecordRepository.AddAsync(weatherRecord);

        return WeatherMapper.FromEntityToResponseDTO(weatherRecord);
    }
}