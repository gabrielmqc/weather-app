using Domain.Utils;

namespace Domain.ValueObjects.Commands;

public record CityLocation : Location
{
    public string Name { get; }
        
    private CityLocation(string name)
    {
        Name = name;
    }
        
    public static CityLocation Create(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "City name cannot be empty");
        return new CityLocation(name);
    }
        
    public override string Describe() => $"City: {Name}";
};