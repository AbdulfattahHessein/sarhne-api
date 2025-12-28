using System.Linq.Expressions;
using System.Security.Claims;
using Api.Extensions;
using Api.Models.Api;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Messages;

public abstract class GetAllMessages : ApiEndpoint
{
    private record Response
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsPublic { get; set; }
        public bool IsFav { get; set; }
        public required string SenderName { get; set; }
        public Guid? SenderId { get; set; }
        public required bool SendAnonymously { get; set; }
    }

    private static readonly Expression<Func<Message, Response>> Selector = message =>
        new()
        {
            Id = message.Id,
            Content = message.Content,
            Date = message.Date,
            IsPublic = message.IsPublic,
            IsFav = message.IsFav,
            SenderName = message.SendAnonymously ? "Anonymous" : message.Sender.Name,
            SenderId = message.SendAnonymously ? null : message.SenderId,
            SendAnonymously = message.SendAnonymously,
        };

    public static readonly Delegate Handler = async (
        [AsParameters] MessagesQueryParams queryParams,
        AppDbContext dbContext,
        ClaimsPrincipal User,
        CancellationToken cancellationToken
    ) =>
    {
        var query = dbContext
            .Messages.OrderByDescending(m => m.Date)
            .ApplyFilter(User.Id, queryParams);

        var count = await query.CountAsync(cancellationToken);

        var messages = await query.Select(Selector).ToListAsync(cancellationToken);

        return Ok(messages, count, queryParams.PageNumber, queryParams.PageSize);
    };
}
