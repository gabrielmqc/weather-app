using System.Net;
using System.Text.Json;
using Domain.Exceptions;
using Infrastructure.Exceptions;

namespace WeatherApp.API.MiddleWares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InfrastructureException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = ex.Message,
                statusCode = ex.StatusCode
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = ex.Message,
                statusCode = ex.StatusCode
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (Exception)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new
            {
                error = "An unexpected error occurred.",
                statusCode = 500
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        
    }
}