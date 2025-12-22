using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Api.Features.Auth;

public static class Logout
{
    public static async Task<IResult> Handler(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return TypedResults.Ok(new { message = "Logged out successfully" });
    }
}
