using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Services.Interfaces;

namespace Api.Services.Implementations;

public class EmailTokenService : IEmailTokenService
{
    private readonly IDataProtector _protector;

    public EmailTokenService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("EmailVerification");
    }

    public string Generate(string email, string securityStamp)
    {
        // We bundle the stamp into the payload
        var payload =
            $"{email}|{DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds()}|{securityStamp}";
        var protectedBytes = _protector.Protect(Encoding.UTF8.GetBytes(payload));

        return WebEncoders.Base64UrlEncode(protectedBytes);
    }

    public bool Validate(string token, string email, string currentSecurityStamp)
    {
        try
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decryptedBytes = _protector.Unprotect(decodedBytes);
            var data = Encoding.UTF8.GetString(decryptedBytes);

            var parts = data.Split('|');
            var tokenEmail = parts[0];
            var expiryUnix = long.Parse(parts[1]);
            var tokenStamp = parts[2]; // The stamp from when the token was made

            var expiry = DateTimeOffset.FromUnixTimeSeconds(expiryUnix);

            // Check 1: Email matches
            // Check 2: Token isn't expired
            // Check 3: Stamp in token matches the one currently in the DB
            return tokenEmail.Equals(email, StringComparison.OrdinalIgnoreCase)
                && expiry > DateTimeOffset.UtcNow
                && tokenStamp == currentSecurityStamp;
        }
        catch
        {
            return false;
        }
    }
}
