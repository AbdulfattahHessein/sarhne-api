using Api.Models.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Api.Features.Auth;

public abstract class Logout : ApiEndpoint
{
    public static readonly Delegate Handler = async (
        HttpContext httpContext,
        CancellationToken _
    ) =>
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    };
}
