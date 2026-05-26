using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IWeatherRecordRepository
{
    Task<WeatherRecord> AddAsync(WeatherRecord weatherRecord);

    Task<List<WeatherRecord>> GetHistoryAsync(
        string city,
        DateTime fromDate);
}