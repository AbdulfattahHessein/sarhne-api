using System.Text.Json;
using Api.Models.Api;
using Microsoft.EntityFrameworkCore;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private Task HandleException(HttpContext context, Exception ex)
    {
        logger.LogError("{Message}", ex.Message);

        var response = ApiResponse.Failure(ex.Message);

        var result = JsonSerializer.Serialize(response, jsonSerializerOptions);

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = ex switch
        {
            UnauthorizedAccessException _ => StatusCodes.Status401Unauthorized,
            DbUpdateConcurrencyException _ => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        return context.Response.WriteAsync(result);
    }
}
