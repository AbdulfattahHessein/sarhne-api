using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsExtensions
{
    extension(ClaimsPrincipal principal)
    {
        public string? Id => principal.FindFirstValue(UserClaimTypes.Id);
        public string? Email => principal.FindFirstValue(UserClaimTypes.Email);
        public string? Name => principal.FindFirstValue(UserClaimTypes.Name);
        public IEnumerable<string>? Roles => principal.FindAll(UserClaimTypes.Roles).Select(r => r.Value);
        public bool IsAuthenticated => principal.Identity?.IsAuthenticated ?? false;
        public bool IsEmailConfirmed => principal.FindFirstValue(UserClaimTypes.IsEmailConfirmed) == true.ToString();
        public string? ProfileSlug => principal.FindFirstValue(UserClaimTypes.ProfileSlug);
    }

}