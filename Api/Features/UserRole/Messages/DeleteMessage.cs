using System.Security.Claims;
using Api.Extensions;
using Api.Features.UserRole.Messages.Extensions;
using Api.Models.Api;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Messages;

public abstract class DeleteMessage : ApiEndpoint
{
    public static async Task<Results<Ok<ApiResponse>, NotFound<ApiResponse>>> Handler(
        Guid id,
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

        dbContext.Messages.Remove(message);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}
