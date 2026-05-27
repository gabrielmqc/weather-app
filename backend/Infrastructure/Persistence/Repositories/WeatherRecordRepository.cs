using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class WeatherRecordRepository : IWeatherRecordRepository
{
    
    private readonly AppDbContext _context;
    public WeatherRecordRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<WeatherRecord> AddAsync(WeatherRecord weatherRecord)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
    
        try
        {
            await _context.WeatherRecords.AddAsync(weatherRecord);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return weatherRecord;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IReadOnlyList<WeatherRecord>> GetHistoryAsync(SearchCriteria criteria, int days = 30)
    {
        DateTime cutoffDate = DateTime.UtcNow.AddDays(-days);

        IQueryable<WeatherRecord> query = _context.WeatherRecords
            .AsNoTracking()
            .Where(w => w.RecordedAt >= cutoffDate);

        query = criteria switch
        {
            CityCriteria city =>
                query.Where(w => w.City == city.City),

            CoordinatesCriteria coordinates =>
                query.Where(w =>
                    w.Latitude == coordinates.Latitude &&
                    w.Longitude == coordinates.Longitude),

            _ => throw new ArgumentOutOfRangeException(
                nameof(criteria),
                "Unsupported search criteria")
        };

        return await query
            .OrderByDescending(w => w.RecordedAt)
            .ToListAsync();
    }
    
}