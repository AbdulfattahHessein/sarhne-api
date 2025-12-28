namespace Api.Models.Api;

public class SuccessResponse<T>(T data, string message = "Succeeded") : ApiResponse(message)
{
    public T Data { get; } = data;

    public static SuccessResponse<T> Success(T data, string message = "Succeeded") =>
        new(data, message);
}
