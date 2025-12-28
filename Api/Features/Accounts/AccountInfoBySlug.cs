using System.Linq.Expressions;
using Api.Models.Api;
using Core.Entities;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Accounts;

public abstract class AccountInfoBySlug : ApiEndpoint
{
    private record Response
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
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
            Settings = new Settings
            {
                AllowMessages = user.Settings.AllowMessages,
                AllowImages = user.Settings.AllowImages,
                AllowAnonymous = user.Settings.AllowAnonymous,
            },
        };

    public static async Task<IResult> Handler(
        string slug,
        AppDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var user = await dbContext
            .Users.Where(u => u.ProfileSlug == slug)
            .Select(Selector)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return NotFound();

        return Ok(user);
    }
}
