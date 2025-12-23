using Core.Entities;
using Core.Enums;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auth;

public static class Register
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

    public static async Task<IResult> Handler(
        Request model,
        SarhneDbContext dbContext,
        IPasswordHasher<User> hasher
    )
    {
        var userExists = dbContext.Users.Any(u => u.Email == model.Email);
        if (userExists)
        {
            return TypedResults.BadRequest(new { message = "User already exists" });
        }

        // Save the user to the database
        var newUser = new User { Email = model.Email };

        var userRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == RoleType.User);

        if (userRole != null)
            newUser.Roles.Add(userRole);

        newUser.PasswordHash = hasher.HashPassword(newUser, model.Password); // In real app, hash the password

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();

        return TypedResults.Ok(new { message = "User registered successfully" });
    }
}
