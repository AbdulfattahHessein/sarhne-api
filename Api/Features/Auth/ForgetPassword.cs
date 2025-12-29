using System.Net;
using Api.Models.Api;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Shared.Services.Interfaces;
using Shared.Templates;

namespace Api.Features.Auth;

public abstract class ForgetPassword : ApiEndpoint
{
    public record Request(string Email);

    public static async Task<NoContent> Handler(
        Request request,
        AppDbContext dbContext,
        IEmailTokenService emailTokenService,
        HttpContext httpContext,
        IEmailService emailService,
        ITemplateService templateService
    )
    {
        var user = await dbContext.Users.FirstAsync(u => u.Email == request.Email);

        var token = emailTokenService.Generate(request.Email, user.SecurityStamp.ToString());

        string origin = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        var resetUrl =
            $"{origin}/api/auth/reset-password"
            + $"?email={request.Email}&token={WebUtility.UrlEncode(token)}";

        var emailBody = await templateService.GetTemplateAsync(
            TemplateConstants.ResetPasswordEmail
        );

        emailBody = templateService.ReplaceInTemplate(
            emailBody,
            new Dictionary<string, string>
            {
                { "{resetUrl}", resetUrl },
                {
                    "{userName}",
                    string.IsNullOrEmpty(user.Name) ? user.Email.Split('@')[0] : user.Name
                },
            }
        );

        // TODO: Send email
        await emailService.SendAsync(user.Email, "Reset Password", emailBody);

        return NoContent();
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator(AppDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Email)
                .MustAsync(
                    async (email, cancellation) =>
                        await dbContext.Users.AnyAsync(
                            u => u.Email == email,
                            cancellationToken: cancellation
                        )
                )
                .WithMessage("User not found.");
        }
    }
}
