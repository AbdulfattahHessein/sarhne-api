using Api.Services.Contracts;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auth;

public static class ConfirmEmail
{
    public record Request(string Email, string Token);

    public static async Task<IResult> Handler(
        [AsParameters] Request request,
        SarhneDbContext dbContext,
        IEmailTokenService emailTokenService
    )
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return TypedResults.BadRequest(new { message = "User not found" });
        }

        var isValid = emailTokenService.Validate(request.Token, request.Email);

        if (!isValid)
        {
            return TypedResults.BadRequest(new { message = "Invalid or expired token" });
        }

        user.IsEmailConfirmed = true;

        await dbContext.SaveChangesAsync();

        return TypedResults.Ok(new { message = "Email verified successfully." });
    }
}
