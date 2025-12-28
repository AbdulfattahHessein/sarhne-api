namespace Core.Models;

public record PaginatedQueryParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
