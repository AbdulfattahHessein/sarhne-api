using Api.Services.Contracts;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auth;

public static class ConfirmEmail
{
    public record Request(string Email, string Token);

    public static async Task<IResult> Handler([AsParameters] Request _, SarhneDbContext dbContext)
    {
        await dbContext.Users.ExecuteUpdateAsync(u =>
            u.SetProperty(user => user.IsEmailConfirmed, true)
        );

        return TypedResults.Ok(new { message = "Email verified successfully." });
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator(SarhneDbContext dbContext, IEmailTokenService emailTokenService)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Token).NotEmpty();

            RuleFor(x => x)
                .CustomAsync(
                    async (request, context, cancellation) =>
                    {
                        var user = await dbContext
                            .Users.Where(u => u.Email == request.Email)
                            .Select(u => new { u.IsEmailConfirmed })
                            .FirstOrDefaultAsync(cancellation);

                        if (user == null)
                        {
                            context.AddFailure(nameof(request.Email), "User not found.");
                            return;
                        }

                        if (user.IsEmailConfirmed)
                        {
                            context.AddFailure(nameof(request.Email), "Email already confirmed.");
                        }
                    }
                );

            RuleFor(x => x)
                .Custom(
                    (request, context) =>
                    {
                        var isValid = emailTokenService.Validate(request.Token, request.Email);

                        if (!isValid)
                        {
                            context.AddFailure(nameof(request.Token), "Invalid or expired token.");
                        }
                    }
                );
        }
    }
}
