using System.Linq.Expressions;
using System.Security.Claims;
using Api.Extensions;
using Api.Models;
using Api.Models.Api;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Messages;

public abstract class SendMessage : ApiEndpoint
{
    public record Request(string Content, Guid ReceiverId, bool SendAnonymously = false);

    public class Validator : AbstractValidator<Request>
    {
        public Validator(AppDbContext dbContext)
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Message content is required.")
                .MaximumLength(1000)
                .WithMessage("Message content cannot exceed 1000 characters.");

            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithMessage("Receiver ID is required.")
                .MustAsync(
                    async (id, cancellation) =>
                        await dbContext.Users.AnyAsync(u => u.Id == id, cancellation)
                )
                .WithMessage("Receiver not found.");
        }
    }

    public static async Task<Created<ApiResponse<BaseResponse>>> Handler(
        Request model,
        AppDbContext dbContext,
        ClaimsPrincipal User,
        CancellationToken cancellationToken
    )
    {
        var senderId = Guid.Parse(User.Id);

        var message = new Message
        {
            Content = model.Content,
            SenderId = senderId,
            ReceiverId = model.ReceiverId,
            SendAnonymously = model.SendAnonymously,
        };

        await dbContext.Messages.AddAsync(message, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Created($"/api/user/messages/{message.Id}", message.Id, "Message sent successfully");
    }
}
