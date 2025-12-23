namespace Api.Models.Api;

public class ErrorResponse(string message, IDictionary<string, string[]>? errors = null)
    : ApiResponse(message)
{
    public IDictionary<string, string[]>? Errors { get; } = errors;

    public static ErrorResponse Failure(
        string message,
        IDictionary<string, string[]>? errors = null
    ) => new(message, errors);
}
