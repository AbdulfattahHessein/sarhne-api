using System.Linq.Expressions;
using System.Security.Claims;
using Api.Extensions;
using Api.Models.Api;
using Core.Entities;
using Core.Enums;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Accounts;

public abstract class GetMyAccount : ApiEndpoint
{
    public record Response
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Bio { get; set; }
        public required string PhoneNumber { get; set; }
        public required string ProfileSlug { get; set; } = string.Empty;

        public required string Gender { get; set; }
        public required Settings Settings { get; set; }
    }

    public record Settings
    {
        public required bool AllowMessages { get; set; }
        public required bool AllowImages { get; set; }
        public required bool AllowAnonymous { get; set; }
    }

    private static readonly Expression<Func<User, Response>> Selector = user =>
        new()
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            Email = user.Email,
            Bio = user.Bio,
            PhoneNumber = user.PhoneNumber,
            ProfileSlug = user.ProfileSlug,
            Gender = user.Gender.ToString(),
            Settings = new Settings
            {
                AllowMessages = user.Settings.AllowMessages,
                AllowImages = user.Settings.AllowImages,
                AllowAnonymous = user.Settings.AllowAnonymous,
            },
        };

    public static async Task<Results<Ok<ApiResponse<Response>>, NotFound<ApiResponse>>> Handler(
        AppDbContext dbContext,
        ClaimsPrincipal user,
        CancellationToken cancellationToken
    )
    {
        var userId = Guid.Parse(user.Id);

        var me = await dbContext
            .Users.Where(u => u.Id == userId)
            .Select(Selector)
            .FirstOrDefaultAsync(cancellationToken);

        if (me is null)
            return NotFound();

        return Ok(me);
    }
}
