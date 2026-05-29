namespace Application.Interfaces.Services;

public interface IWeatherProviderFactory
{
    IWeatherProvider GetProvider();

}