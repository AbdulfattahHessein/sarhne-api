using System.Net;
using Api.Models.Api;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Services.Interfaces;
using Shared.Templates;

namespace Api.Features.Auth;

public abstract class SendEmailVerification : ApiEndpoint
{
    public record Request(string Email);

    public class Validator : AbstractValidator<Request>
    {
        public Validator(AppDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Email)
                .MustAsync(
                    async (email, cancellation) =>
                    {
                        var user = await dbContext.Users.FirstOrDefaultAsync(
                            u => u.Email == email,
                            cancellationToken: cancellation
                        );

                        return user != null;
                    }
                )
                .WithMessage("User not found.");
        }
    }

    public static async Task<IResult> Handler(
        Request request,
        AppDbContext dbContext,
        IEmailService emailService,
        ITemplateService templateService,
        IEmailTokenService emailTokenService,
        HttpContext httpContext
    )
    {
        var user = await dbContext.Users.FirstAsync(u => u.Email == request.Email);

        var token = emailTokenService.Generate(request.Email, user.SecurityStamp.ToString());

        string origin = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        var verifyUrl =
            $"{origin}/api/auth/email-verification"
            + $"?email={request.Email}&token={WebUtility.UrlEncode(token)}";

        var emailBody = await templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

        emailBody = templateService.ReplaceInTemplate(
            emailBody,
            new Dictionary<string, string>
            {
                { "{verifyUrl}", verifyUrl },
                { "{email}", request.Email },
                {
                    "{userName}",
                    string.IsNullOrEmpty(user.Name) ? user.Email.Split('@')[0] : user.Name
                },
            }
        );

        await emailService.SendAsync(request.Email, "Verify your email", emailBody);

        return Ok("Verification email sent.");
    }
}
