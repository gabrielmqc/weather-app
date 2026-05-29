using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Mappers;

namespace Application.UseCases.Weather;

public class GetWeatherHistoryUseCase
{
    private readonly IWeatherRecordRepository _weatherRecordRepository;

    public GetWeatherHistoryUseCase(IWeatherRecordRepository weatherRecordRepository)
    {
        _weatherRecordRepository = weatherRecordRepository;
    }

    public async Task<IReadOnlyList<WeatherRecordDTO>> Execute(WeatherHistoryQuery query)
    {
        var criteria = query.SearchBy;
        
        var weatherRecords = await _weatherRecordRepository.GetHistoryAsync(criteria);
        
        return WeatherMapper.FromEntityListToResponseDTOList(weatherRecords);
    }
}