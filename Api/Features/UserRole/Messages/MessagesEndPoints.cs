using Api.Extensions;
using Api.Interfaces;
using Core.Enums;

namespace Api.Features.UserRole.Messages;

public class MessagesEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var userMessages = app.MapGroup("/user")
            .MapGroup("/messages")
            .WithTags("User messages")
            .RequireRoles(RoleType.User);

        userMessages.MapGet("/", GetAllMessages.Handler);

        userMessages.MapPost("/", SendMessage.Handler);

        userMessages.MapDelete("/{id:guid}", DeleteMessage.Handler);

        userMessages.MapPatch("/{id:guid}/favorite", ToggleFavoriteMessage.Handler);
    }
}
