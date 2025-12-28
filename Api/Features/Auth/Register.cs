using Api.Models.Api;
using Core.Entities;
using Core.Enums;
using FluentValidation;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using Shared.Services.Interfaces;

namespace Api.Features.Auth;

public abstract class Register : ApiEndpoint
{
    public record Request(string Email, string Password);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("A valid email is required.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }

    public static readonly Delegate Handler = async Task<
        Results<BadRequest<ApiResponse>, NoContent>
    > (Request model, AppDbContext dbContext, IPasswordHasher<User> hasher) =>
    {
        var userExists = dbContext.Users.Any(u => u.Email == model.Email);
        if (userExists)
        {
            return BadRequest("User already exists");
        }

        // Save the user to the database
        var newUser = new User { Email = model.Email, Name = model.Email.Split('@')[0] };

        var userRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == RoleType.User);

        if (userRole != null)
            newUser.Roles.Add(userRole);

        newUser.PasswordHash = hasher.HashPassword(newUser, model.Password); // In real app, hash the password

        await dbContext.Users.AddAsync(newUser);

        await dbContext.Users.GenerateSlugAsync(newUser);

        await dbContext.SaveChangesAsync();

        return NoContent();
    };
}
