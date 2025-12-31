using System.Security.Claims;
using Api.Extensions;
using Api.Models;
using Api.Models.Api;
using Core.Entities;
using Core.Enums;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.UserRole.Settings;

public class ChangePersonalInformation : ApiEndpoint
{
    public record Request(string Name, string Gender, string Bio);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode("ERR_REQUIRED").MaximumLength(100);
            RuleFor(x => x.Bio).MaximumLength(1000).WithErrorCode("ERR_MAX_LENGTH1000");
        }
    }

    private static void UpdateUser(User user, Request request)
    {
        user.Name = request.Name;
        user.Gender = request.Gender == "Male" ? Gender.Male : Gender.Female;
        user.Bio = request.Bio;
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

public class ChangeProfileSlug : ApiEndpoint
{
    public record Request(string ProfileSlug);

    public class Validator : AbstractValidator<Request>
    {
        public Validator(AppDbContext dbContext)
        {
            RuleFor(x => x.ProfileSlug)
                .NotEmpty()
                .WithErrorCode("ERR_REQUIRED")
                .MustAsync(
                    (slug, cancellation) =>
                    {
                        return dbContext.Users.AnyAsync(u => u.ProfileSlug == slug, cancellation);
                    }
                )
                .WithErrorCode("ERR_ALREADY_USED")
                .Matches(@"^[a-zA-Z0-9_]*$")
                .WithErrorCode("ERR_INVALID")
                .MaximumLength(100)
                .WithErrorCode("ERR_MAX_LENGTH100");
        }
    }

    public static void UpdateUser(User user, Request request)
    {
        user.ProfileSlug = request.ProfileSlug;
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
