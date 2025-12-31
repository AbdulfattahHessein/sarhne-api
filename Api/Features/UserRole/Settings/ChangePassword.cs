using System.Security.Claims;
using Api.Extensions;
using Api.Models.Api;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Settings;

public class ChangePassword : ApiEndpoint
{
    public record Request(string Password);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Password).NotEmpty().WithErrorCode("ERR_REQUIRED");
        }
    }

    public static async Task<Results<Ok<ApiResponse>, NotFound<ApiResponse>>> Handler(
        Request model,
        AppDbContext dbContext,
        IPasswordHasher<User> PasswordHasher,
        ClaimsPrincipal user
    )
    {
        var userId = Guid.Parse(user.Id);

        var userToUpdate = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (userToUpdate == null)
        {
            return NotFound();
        }

        userToUpdate.PasswordHash = PasswordHasher.HashPassword(userToUpdate, model.Password);

        await dbContext.SaveChangesAsync();

        return Ok();
    }
}
