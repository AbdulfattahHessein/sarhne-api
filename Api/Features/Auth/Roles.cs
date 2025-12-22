using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auth;

public static class Roles
{
    public static async Task<IResult> Handler(SarhneDbContext dbContext)
    {
        var users = await dbContext
            .Users.Select(u => new
            {
                u.Id,
                u.Email,
                Roles = u.Roles.Select(r => r.Name.ToString()).ToList(),
            })
            .ToListAsync();

        return TypedResults.Ok(users);
    }
}
