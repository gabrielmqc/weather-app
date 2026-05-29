namespace Application.DTOs.Response;

public record CoordinatesResponseDTO(string Name,
    double Lat,
    double Lon,
    string Country,
    string State);