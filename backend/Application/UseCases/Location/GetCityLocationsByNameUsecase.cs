using Application.DTOs.Response;
using Application.Interfaces.Services;
using Domain.ValueObjects.Commands;

namespace Application.UseCases.Location;

public class GetCityLocationsByNameUsecase
{
    private readonly ICoordinatesProvider _coordinatesProvider;

    public GetCityLocationsByNameUsecase(ICoordinatesProvider coordinatesProvider)
    {
        _coordinatesProvider = coordinatesProvider;
    }

    public async Task<List<CoordinatesResponseDTO>> Execute(string cityName)
    {
        CityLocation location = CityLocation.Create(cityName);
        
        return await _coordinatesProvider.GetCoordinatesByCityAsync(location.Name);

    }
}