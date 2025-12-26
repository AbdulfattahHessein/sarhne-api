using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;

namespace Infrastructure.Extensions;

public static class UserExtensions
{
    extension(IQueryable<User> users)
    {
        public async Task<string> GenerateSlugAsync(User user)
        {
            string baseSlug = SlugHelper.GenerateSlug(user.Name);
            string slug = baseSlug;
            int counter = 1;

            while (await users.AnyAsync(u => u.ProfileSlug == slug))
            {
                slug = $"{baseSlug}-{counter}";

                counter++;
            }

            user.ProfileSlug = slug;

            return slug;
        }
    }
}
