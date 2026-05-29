using Application.Interfaces.Services;
using Infrastructure.Providers.MockProviders;
using Infrastructure.Providers.OpenWeather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Providers.Factories;

public class WeatherProviderFactory : IWeatherProviderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public WeatherProviderFactory(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public IWeatherProvider GetProvider()
{
    var providerName = _configuration["FeatureFlags:WeatherProvider"];

    Console.WriteLine("================================");
    Console.WriteLine($"PROVIDER ESCOLHIDO: {providerName}");
    Console.WriteLine("================================");

    return providerName?.ToLower() switch
    {
        "openweather" => _serviceProvider.GetRequiredService<OpenWeatherProvider>(),
        "mock" => _serviceProvider.GetRequiredService<MockWeatherProvider>(),
        _ => _serviceProvider.GetRequiredService<OpenWeatherProvider>()
    };
}
}