using Api.Models.Api;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shared.Services.Interfaces;

namespace Api.Features.Auth;

public abstract class ConfirmEmail : ApiEndpoint
{
    public record Request(string Email, string Token);

    public static async Task<NoContent> Handler([AsParameters] Request _, AppDbContext dbContext)
    {
        await dbContext.Users.ExecuteUpdateAsync(u =>
            u.SetProperty(user => user.IsEmailConfirmed, true)
        );

        return NoContent();
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator(AppDbContext dbContext, IEmailTokenService emailTokenService)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Token).NotEmpty();

            RuleFor(x => x)
                .CustomAsync(
                    async (request, context, cancellation) =>
                    {
                        var user = await dbContext.Users.FirstOrDefaultAsync(
                            u => u.Email == request.Email,
                            cancellation
                        );

                        if (user == null)
                        {
                            context.AddFailure(nameof(request.Email), "User not found.");
                            return;
                        }

                        if (user.IsEmailConfirmed)
                        {
                            context.AddFailure(nameof(request.Email), "Email already confirmed.");
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
        }
    }
}
