using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IWeatherRecord
{
    Task AddAsync(WeatherRecord weatherRecord);

    Task<List<WeatherRecord>> GetHistoryAsync(
        string city,
        DateTime fromDate);
}