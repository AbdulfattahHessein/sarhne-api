using Api.Extensions;
using Api.Interfaces;
using Core.Enums;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Api.Features.Auth;

public class AuthEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/auth").WithTags("Auth");

        authGroup.MapPost("/register", Register.Handler);

        authGroup.MapPost("/login", Login.Handler);

        authGroup.MapPost("/logout", (Delegate)Logout.Handler);

        authGroup.MapPost("/email-verification", SendEmailVerification.Handler);

        authGroup.MapGet("/email-verification", ConfirmEmail.Handler);

        authGroup.MapGet("/user-info", UserInfo.Handler);
        // .RequireRoles(RoleType.User, RoleType.Admin);

        // authGroup.MapGet("/roles", Roles.Handler).RequireRoles(RoleType.User, RoleType.Admin);
    }
}
