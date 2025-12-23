namespace Api.Models.Api;

public class SuccessResponse<T>(T data, string message = "Operation succeeded")
    : ApiResponse(true, message)
{
    public T Data { get; } = data;

    public static SuccessResponse<TData> Success<TData>(
        TData data,
        string message = "Operation succeeded"
    ) => new(data, message);
}
