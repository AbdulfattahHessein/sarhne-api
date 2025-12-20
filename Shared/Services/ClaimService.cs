using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class ClaimService(IHttpContextAccessor httpContextAccessor) : IClaimService
{
    public int UserId => int.Parse(GetUserId());

    private string GetUserId()
    {
        return GetClaim(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID is not found");
    }

    public string? GetClaim(string key)
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value;
    }
}
