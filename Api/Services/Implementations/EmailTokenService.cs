using Api.Services.Contracts;
using Microsoft.AspNetCore.DataProtection;

namespace Api.Services.Implementations;

public class EmailTokenService : IEmailTokenService
{
    private readonly IDataProtector _protector;

    public EmailTokenService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("EmailVerification");
    }

    public string Generate(string email)
    {
        var payload = $"{email}|{DateTime.UtcNow.AddHours(24):O}";
        return _protector.Protect(payload);
    }

    public bool Validate(string token, string email)
    {
        try
        {
            var data = _protector.Unprotect(token);
            var parts = data.Split('|');

            var tokenEmail = parts[0];
            var expiry = DateTime.Parse(parts[1]);

            return tokenEmail == email && expiry > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }
}
