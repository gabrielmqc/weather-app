using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Providers.OpenWeather;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString(
                    "DefaultConnection"),
                b => b.MigrationsAssembly(
                    typeof(AppDbContext).Assembly.FullName)));

        services.Configure<OpenWeatherOptions>(
            configuration.GetSection(
                OpenWeatherOptions.SectionName));

        services.AddHttpClient<
            IWeatherProvider,
            OpenWeatherProvider>();

        services.AddScoped<IWeatherRecordRepository, WeatherRecordRepository>();

        return services;
    }
}