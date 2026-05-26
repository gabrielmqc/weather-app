using Domain.Exceptions;

namespace Domain.Utils;

public class Guard
{
    public static void AgainstNullOrWhiteSpace(
        string value,
        string message)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(message);
    }

    public static void AgainstOutOfRange(
        double value,
        double min,
        double max,
        string message)
    {
        if (value < min || value > max)
            throw new DomainException(message);
    }
}