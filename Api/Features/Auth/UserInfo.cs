using System.Security.Claims;
using Api.Models.Api;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auth;

public abstract class UserInfo : ApiEndpoint
{
    public static async Task<IResult> Handler(HttpContext httpContext, AppDbContext dbContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Ok("User not logged in.");
        }

        var user = await dbContext
            .Users.AsNoTracking()
            .Select(user => new
            {
                user.Id,
                user.Email,
                user.IsEmailConfirmed,
                Roles = user.Roles.Select(r => r.Name.ToString()).ToList(),
            })
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

        if (user == null)
        {
            await httpContext.SignOutAsync();

            return Ok("User not logged in.");
        }

        return Ok(user);
    }
}
