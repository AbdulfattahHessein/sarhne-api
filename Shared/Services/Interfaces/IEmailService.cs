using Shared.Models.Email;

namespace Shared.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
