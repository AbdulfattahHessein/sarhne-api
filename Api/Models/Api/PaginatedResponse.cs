namespace Api.Models.Api;

public class PaginatedResponse<T> : SuccessResponse<T>
{
    public Paginator Paginator { get; }

    public PaginatedResponse(
        T data,
        Paginator paginator,
        string message = "Succeeded with pagination"
    )
        : base(data, message)
    {
        Paginator = paginator;
    }

    public PaginatedResponse(
        T data,
        int totalCount,
        int currentPage = 1,
        int pageSize = 10,
        string message = "Succeeded with pagination"
    )
        : base(data, message)
    {
        Paginator = new Paginator(totalCount, currentPage, pageSize);
    }

    public static PaginatedResponse<TData> Success<TData>(
        TData data,
        Paginator paginator,
        string message = "Succeeded with pagination"
    ) => new(data, paginator, message);

    public static PaginatedResponse<TData> Success<TData>(
        TData data,
        int totalCount,
        int currentPage = 1,
        int pageSize = 10,
        string message = "Succeeded with pagination"
    ) => new(data, totalCount, currentPage, pageSize, message);
}
