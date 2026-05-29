using Application.Interfaces.Repositories;

namespace Application.UseCases.Weather;

public class GetAllCitiesRegisteredUseCase
{
    private readonly IWeatherRecordRepository _repository;

    public GetAllCitiesRegisteredUseCase(IWeatherRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<string>> Execute()
    {
        return await _repository.GetAllCitiesRegistered();
    }
}