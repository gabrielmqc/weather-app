namespace Application.DTOs.Response;

public record WeatherRecordDTO(Guid Id, string City, double Temperature, double Latitude, double Longitude, DateTime RecordedAt );