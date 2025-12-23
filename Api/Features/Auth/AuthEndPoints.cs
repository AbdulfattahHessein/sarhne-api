using Api.Interfaces;

namespace Api.Features.Auth;

public class AuthEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/auth").WithTags("Auth");

        authGroup.MapPost("/register", Register.Handler);

        authGroup.MapPost("/login", Login.Handler);

        authGroup.MapPost("/logout", Logout.Handler);

        authGroup.MapPost("/email-verification", SendEmailVerification.Handler);

        authGroup.MapGet("/email-verification", ConfirmEmail.Handler);

        authGroup.MapGet("/user-info", UserInfo.Handler);

        authGroup.MapPost("/forget-password", ForgetPassword.Handler);

        authGroup.MapPost("/reset-password", ResetPassword.Handler);
    }
}
