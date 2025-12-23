namespace Shared.Services.Interfaces;

public interface IEmailTokenService
{
    string Generate(string email, string securityStamp);
    bool Validate(string token, string email, string currentSecurityStamp);
}
