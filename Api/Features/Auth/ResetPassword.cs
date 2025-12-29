using Api.Models.Api;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Services.Interfaces;

namespace Api.Features.Auth;

public abstract class ResetPassword : ApiEndpoint
{
    public record Request(string Email, string Token, string Password);

    public static async Task<NoContent> Handler(
        Request request,
        AppDbContext dbContext,
        IPasswordHasher<User> PasswordHasher
    )
    {
        var user = await dbContext.Users.FirstAsync(u => u.Email == request.Email);

        user.PasswordHash = PasswordHasher.HashPassword(user, request.Password); // In real app, hash the password

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator(AppDbContext dbContext, IEmailTokenService emailTokenService)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x)
                .CustomAsync(
                    async (request, context, cancellation) =>
                    {
                        var user = await dbContext.Users.FirstOrDefaultAsync(
                            u => u.Email == request.Email,
                            cancellationToken: cancellation
                        );

                        if (user == null)
                        {
                            context.AddFailure(nameof(request.Email), "User not found.");
                            return;
                        }

                        var isValid = emailTokenService.Validate(
                            request.Token,
                            request.Email,
                            user.SecurityStamp.ToString()
                        );

                        if (!isValid)
                        {
                            context.AddFailure(nameof(request.Token), "Invalid or expired token.");
                        }
                    }
                );

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
