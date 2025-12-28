using Core.Entities;
using Core.Models;
using Infrastructure.Extensions;

namespace Api.Features.UserRole.Messages;

public class MessageType
{
    public const string All = "All",
        Sent = "Sent",
        Received = "Received";
}

public record MessagesQueryParams : PaginatedQueryParams
{
    public string? Search { get; set; }
    public string? Type { get; set; }
    public bool IsFav { get; set; } = false;
}

public static class MessagesQueryParamsExtensions
{
    public static IQueryable<Message> ApplyFilter(
        this IQueryable<Message> query,
        string userId,
        MessagesQueryParams queryParams
    )
    {
        query = query.PageBy(queryParams.PageNumber, queryParams.PageSize);

        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            query = query.Where(x => x.Content.Contains(queryParams.Search));
        }

        if (!string.IsNullOrEmpty(queryParams.Type) && queryParams.Type == MessageType.Sent)
        {
            query = query.Where(x => x.Sender.Id.ToString() == userId);
        }

        if (!string.IsNullOrEmpty(queryParams.Type) && queryParams.Type == MessageType.Received)
        {
            query = query.Where(x => x.Receiver.Id.ToString() == userId);
        }

        if (!string.IsNullOrEmpty(queryParams.Type) && queryParams.Type == MessageType.All)
        {
            query = query.Where(x =>
                x.Sender.Id == Guid.Parse(userId) || x.Receiver.Id == Guid.Parse(userId)
            );
        }

        if (queryParams.IsFav)
        {
            query = query.Where(x => x.IsFav);
        }

        return query;
    }
}
