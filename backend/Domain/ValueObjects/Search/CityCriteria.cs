using Domain.Utils;

namespace Domain.ValueObjects.Search;

public record CityCriteria(string City) : SearchCriteria
{
    public override string Describe() => $"City: {City}";

    public static CityCriteria Create(string city)
    {
        Guard.AgainstNullOrWhiteSpace(
            city,
            "City cannot be empty");

        return new CityCriteria(
            StringNormalizer.Normalize(city));
    }
}