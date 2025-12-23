using System.Security.Claims;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;

namespace Api.Features.Auth;

public class UserInfo
{
    public static async Task<IResult> Handler(HttpContext httpContext, SarhneDbContext dbContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return TypedResults.Unauthorized();
        }

        var user = dbContext
            .Users.Select(user => new
            {
                user.Id,
                user.Email,
                user.IsEmailConfirmed,
                Roles = user.Roles.Select(r => r.Name.ToString()).ToList(),
            })
            .FirstOrDefault(u => u.Id.ToString() == userId);

        if (user == null)
        {
            await httpContext.SignOutAsync();

            return TypedResults.NotFound("User not found.");
        }

        return TypedResults.Ok(user);
    }
}
