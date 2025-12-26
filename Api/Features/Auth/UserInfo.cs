using System.Linq.Expressions;
using System.Security.Claims;
using Api.Extensions;
using Api.Models.Api;
using Core.Entities;

namespace Api.Features.Auth;

public abstract class UserInfo : ApiEndpoint
{
    private record Response
    {
        public required string? Id { get; set; }
        public required string? Email { get; set; }
        public required bool? IsEmailConfirmed { get; set; }
        public required IEnumerable<string>? Roles { get; set; }
        public string? Name { get; set; }
        public string? ProfileSlug { get; set; }
    }

    private static readonly Expression<Func<User, Response>> Selector = user =>
        new()
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            IsEmailConfirmed = user.IsEmailConfirmed,
            Roles = user.Roles.Select(r => r.Name.ToString()).ToList(),
            Name = user.Name,
        };

    public static async Task<IResult> Handler(ClaimsPrincipal User)
    {
        if (!User.IsAuthenticated)
        {
            return Ok("User not logged in.");
        }

        var user = new Response
        {
            Id = User.Id,
            Email = User.Email,
            Roles = User.Roles,
            Name = User.Name,
            IsEmailConfirmed = User.IsEmailConfirmed,
            ProfileSlug = User.ProfileSlug,
        };

        return Ok(user);
    }
}
