using Core.Constants;

namespace Api.Models.Api;

public record Paginator(int TotalCount, int PageNumber = 1, int PageSize = 10)
{
    public int PageNumber { get; } = Math.Max(AppConstants.MIN_PAGE_NUMBER, PageNumber);
    public int PageSize { get; } = Math.Min(PageSize, AppConstants.MAX_PAGE_SIZE);
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1 && TotalPages > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
