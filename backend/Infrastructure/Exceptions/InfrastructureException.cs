namespace Infrastructure.Exceptions;

public class InfrastructureException : Exception
{
    public int StatusCode { get; }

    public InfrastructureException(
        string message,
        int statusCode = 500)
        : base(message)
    {
        StatusCode = statusCode;
    }
}