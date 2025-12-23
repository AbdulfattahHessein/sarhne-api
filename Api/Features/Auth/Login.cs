using System.Net;
using System.Security.Claims;
using Api.Models.Api;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auth;

public abstract class Login : ApiEndpoint
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
        Request request,
        AppDbContext dbContext,
        ClaimsPrincipal User,
        IPasswordHasher<User> hasher,
        HttpContext httpContext
    )
    {
        if (
            User.Identity?.IsAuthenticated == true
            && User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value == request.Email
        )
        {
            return BadRequest("You are already logged in.");
        }

        var user = await dbContext
            .Users.Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            return NotFound();

        // Verify the password
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return BadRequest("Invalid credentials");
        }

        List<Claim> claims =
        [
            new(ClaimTypes.Email, request.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        ];

        foreach (var userRole in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Name.ToString()));
        }

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true, // Survive browser restart
            AllowRefresh = true, // Allow the sliding timer to reset on activity
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
        };

        // 2. This sends the 'Set-Cookie' header to the client
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties
        );

        return NoContent();
    }
}
