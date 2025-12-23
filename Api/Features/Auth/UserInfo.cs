using System.Security.Claims;
using Api.Models.Api;
using Api.Services.Implementations;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Features.Auth;

public abstract class UserInfo : ApiEndpoint
{
    public static async Task<IResult> Handler(HttpContext httpContext, AppDbContext dbContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return NotFound();
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

            return NotFound();
        }

        return Ok(user);
    }
}
