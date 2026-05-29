using Domain.Entities;
using Domain.ValueObjects.Search;

namespace Application.Interfaces.Repositories;

public interface IWeatherRecordRepository
{
    Task<WeatherRecord> AddAsync(WeatherRecord weatherRecord);

    Task<IReadOnlyList<WeatherRecord>> GetHistoryAsync(
        SearchCriteria criteria, 
        int days = 30);

    Task<IReadOnlyList<string>> GetAllCitiesRegistered();
}