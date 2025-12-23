namespace Core.Interfaces;

public interface IApiResponse
{
    bool IsSuccess { get; }
    string Message { get; }
}
