using Core.Interfaces;

namespace Api.Models.Api;

// public class ApiResponse(string message) : IApiResponse
// {
//     public string Message { get; } = message;

//     public static ApiResponse Success(string message = "Succeeded") => new(message);

//     public static ApiResponse Failure(string message) => new(message);
// }

public class ApiResponse<T> : IApiResponse
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public Paginator? Paginator { get; set; }

    public IDictionary<string, string[]>? Errors { get; set; }
}

public class ApiResponse : ApiResponse<object> { }

public static class ApiResponses
{
    public static ApiResponse<T> Success<T>(T data, string message = "Succeeded") =>
        new() { Data = data, Message = message };

    public static ApiResponse Success(string message = "Succeeded") => new() { Message = message };

    public static ApiResponse<T> Success<T>(
        T data,
        int totalCount,
        int currentPage = 1,
        int pageSize = 10,
        string message = "Succeeded"
    ) =>
        new()
        {
            Data = data,
            Message = message,
            Paginator = new Paginator(totalCount, currentPage, pageSize),
        };

    public static ApiResponse Failure(string message = "Failed") => new() { Message = message };

    public static ApiResponse Failure(
        IDictionary<string, string[]> errors,
        string message = "Failed"
    ) => new() { Message = message, Errors = errors };
}
