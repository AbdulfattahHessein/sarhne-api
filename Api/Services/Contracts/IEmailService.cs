namespace Api.Services.Contracts;

public interface ISmtpEmailService
{
    Task SendAsync(string to, string subject, string htmlBody);
}
