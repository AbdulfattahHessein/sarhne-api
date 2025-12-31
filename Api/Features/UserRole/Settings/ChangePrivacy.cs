using System.Security.Claims;
using Api.Extensions;
using Api.Models.Api;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Settings;

public abstract class ChangePrivacy : ApiEndpoint
{
    public record Request(bool AllowMessages, bool AllowImages, bool AllowAnonymous);

    public static void Update(User user, Request request)
    {
        user.Settings.AllowMessages = request.AllowMessages;
        user.Settings.AllowImages = request.AllowImages;
        user.Settings.AllowAnonymous = request.AllowAnonymous;
    }

    public static async Task<Results<Ok<ApiResponse>, NotFound<ApiResponse>>> Handler(
        Request model,
        AppDbContext dbContext,
        ClaimsPrincipal user
    )
    {
        var userId = Guid.Parse(user.Id);

        var userToUpdate = await dbContext
            .Users.Include(u => u.Settings)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (userToUpdate == null)
        {
            return NotFound();
        }

        Update(userToUpdate, model);

        await dbContext.SaveChangesAsync();

        return Ok();
    }
}
