using Application.UseCases.Location;
using Application.UseCases.Weather;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<RegisterCurrentWeatherByCityNameUseCase>();

        services.AddScoped<GetWeatherHistoryUseCase>();

        services.AddScoped<RegisterCurrentWeatherByCoordinatesUseCase>();
        
        services.AddScoped<GetCityLocationsByNameUsecase>();
        
        return services;
    }
}