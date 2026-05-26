using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories;

public class WeatherDataRepository : IWeatherRecord
{
    
    private readonly AppDbContext _context;
    public WeatherDataRepository(AppDbContext context)
    {
        _context = context;
    }
    public Task<WeatherRecord> AddAsync(WeatherRecord weatherRecord)
    {
        throw new NotImplementedException();
    }

    public Task<List<WeatherRecord>> GetHistoryAsync(string city, DateTime fromDate)
    {
        throw new NotImplementedException();
    }
}