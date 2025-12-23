using Core.Interfaces;

namespace Api.Models.Api;

public class ApiResponse(string message) : IApiResponse
{
    public string Message { get; } = message;

    public static ApiResponse Success(string message = "Succeeded") => new(message);

    public static ApiResponse Failure(string message) => new(message);
}
