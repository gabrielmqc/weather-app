using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Integration.Tests;

public class WeatherAppFactory : WebApplicationFactory<Program>
{
    public Mock<IWeatherProvider> WeatherProviderMock { get; } = new();
    public Mock<ICoordinatesProvider> CoordinatesProviderMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
             
            services.RemoveAll<AppDbContext>();
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            services.RemoveAll<IWeatherRecordRepository>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("WeatherAppTestDb");
            });

            services.AddScoped<IWeatherRecordRepository, WeatherRecordRepository>();

            RemoveService<IWeatherProvider>(services);
            RemoveService<ICoordinatesProvider>(services);

            var factoryMock = new Mock<IWeatherProviderFactory>();
            factoryMock.Setup(f => f.GetProvider()).Returns(WeatherProviderMock.Object);
            services.AddScoped(_ => factoryMock.Object);


            services.AddScoped<ICoordinatesProvider>(_ =>
                CoordinatesProviderMock.Object);

            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }

    private static void RemoveService<T>(IServiceCollection services)
        where T : class
    {
        var descriptors = services
            .Where(x => x.ServiceType == typeof(T))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}