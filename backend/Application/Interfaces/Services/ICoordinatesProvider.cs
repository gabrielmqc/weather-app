using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;

public interface ICoordinatesProvider
{
    Task<List<CoordinatesResponseDTO>> GetCoordinatesByCityAsync(string city);
}