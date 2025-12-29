using Api.Models.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Features.Auth;

public abstract class Logout : ApiEndpoint
{
    public static async Task<NoContent> Handler(HttpContext httpContext, CancellationToken _)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }
}
