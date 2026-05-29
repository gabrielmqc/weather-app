using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Providers;
using Infrastructure.Providers.Factories;
using Infrastructure.Providers.MockProviders;
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
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(
                    typeof(AppDbContext).Assembly.FullName)));

        // OpenWeather Configuration
        services.Configure<OpenWeatherOptions>(
            configuration.GetSection(OpenWeatherOptions.SectionName));

        // Registrar os providers individuais
        services.AddHttpClient<OpenWeatherProvider>();
        services.AddScoped<MockWeatherProvider>();

        // Registrar OpenWeatherProvider como ICoordinatesProvider (já que implementa ambos)
        services.AddScoped<ICoordinatesProvider>(sp => 
            sp.GetRequiredService<OpenWeatherProvider>());

        // Feature Flag - Factory para escolher o provider
        services.AddScoped<IWeatherProviderFactory, WeatherProviderFactory>();

        // Repositories
        services.AddScoped<IWeatherRecordRepository, WeatherRecordRepository>();
        
        return services;
    }
}