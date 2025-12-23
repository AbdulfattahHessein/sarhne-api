using Core.Interfaces;

namespace Api.Models.Api;

public class ApiResponse(bool isSuccess, string message) : IApiResponse
{
    public bool IsSuccess { get; } = isSuccess;

    public string Message { get; } = message;

    public static ApiResponse Success(string message = "Operation succeeded") => new(true, message);

    public static ApiResponse Failure(string message) => new(false, message);
}
