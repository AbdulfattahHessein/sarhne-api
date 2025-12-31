using System.Security.Claims;
using Api.Extensions;
using Api.Features.UserRole.Messages.Extensions;
using Api.Models.Api;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Messages;

public abstract class ToggleFavoriteMessage : ApiEndpoint
{
    public record Request(bool IsFav);

    public static async Task<Results<Ok<ApiResponse>, NotFound<ApiResponse>>> Handler(
        Guid id,
        [FromBody] Request model,
        AppDbContext dbContext,
        ClaimsPrincipal user,
        CancellationToken cancellationToken
    )
    {
        var userId = Guid.Parse(user.Id);

        var message = await dbContext
            .Messages.FilterByUser(userId)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (message is null)
            return NotFound();

        await dbContext.Messages.ExecuteUpdateAsync(
            u => u.SetProperty(m => m.IsFav, model.IsFav),
            cancellationToken
        );

        return Ok();
    }
}
