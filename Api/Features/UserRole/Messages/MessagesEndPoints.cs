using Api.Extensions;
using Api.Interfaces;
using Core.Enums;

namespace Api.Features.UserRole.Messages;

public class MessagesEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var userMessages = app.MapGroup("/user/messages")
            .WithTags("User")
            .RequireRoles(RoleType.User);

        userMessages.MapGet("/", GetAllMessages.Handler);

        userMessages.MapPost("/", SendMessage.Handler);
    }
}
