using System.Net;
using Api.Services.Contracts;
using Api.Templates;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.Services.Interfaces;

namespace Api.Features.Auth;

public static class SentEmailVerification
{
    public record Request(string Email);

    public static async Task<IResult> Handler(
        SarhneDbContext dbContext,
        Request request,
        ISmtpEmailService emailService,
        ITemplateService templateService,
        IEmailTokenService emailTokenService,
        HttpContext httpContext
    )
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return TypedResults.BadRequest(new { message = "User not found" });
        }

        var token = emailTokenService.Generate(request.Email);

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

        return TypedResults.Ok(new { message = "Verification email sent." });
    }
}
