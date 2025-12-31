using System.Security.Claims;
using Api.Extensions;
using Api.Models.Api;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Settings;

public class ChangeEmail : ApiEndpoint
{
    public record Request(string Email);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithErrorCode("ERR_REQUIRED")
                .EmailAddress()
                .WithErrorCode("ERR_EMAIL");
        }
    }

    private static void UpdateUser(User user, Request request)
    {
        user.Email = request.Email;
    }

    public static async Task<Results<Ok<ApiResponse>, NotFound<ApiResponse>>> Handler(
        Request model,
        AppDbContext dbContext,
        ClaimsPrincipal user
    )
    {
        var userId = Guid.Parse(user.Id);

        var userToUpdate = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (userToUpdate == null)
        {
            return NotFound();
        }

        UpdateUser(userToUpdate, model);

        await dbContext.SaveChangesAsync();

        return Ok();
    }
}
