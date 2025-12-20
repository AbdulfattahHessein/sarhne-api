namespace Shared.Services.Interfaces;

public interface IClaimService
{
    public int UserId { get; }

    string? GetClaim(string key);
}
