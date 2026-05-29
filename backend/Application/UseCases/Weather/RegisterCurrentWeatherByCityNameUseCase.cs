using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using Domain.ValueObjects.Commands;

namespace Application.UseCases.Weather;

public class RegisterCurrentWeatherByCityNameUseCase
{
    private readonly IWeatherProviderFactory _weatherProviderFactory;
    private readonly IWeatherRecordRepository _weatherRecordRepository;

    public RegisterCurrentWeatherByCityNameUseCase(
        IWeatherProviderFactory weatherProviderFactory,
        IWeatherRecordRepository weatherRecordRepository)
    {
        _weatherProviderFactory = weatherProviderFactory;
        _weatherRecordRepository = weatherRecordRepository;
    }

    public async Task<WeatherRecordDTO> Execute(string cityName)
    {
        CityLocation location = CityLocation.Create(cityName);
        
        var weatherProvider = _weatherProviderFactory.GetProvider();
        WeatherDataProviderDTO weatherData = await weatherProvider.GetWeatherDataByCityAsync(location.Name);

        Domain.Entities.WeatherRecord weatherRecord = WeatherMapper.ToEntity(weatherData);
        await _weatherRecordRepository.AddAsync(weatherRecord);

        return WeatherMapper.FromEntityToResponseDTO(weatherRecord);
    }
}