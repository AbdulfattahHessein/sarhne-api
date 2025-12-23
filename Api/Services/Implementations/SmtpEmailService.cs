using System.Net;
using System.Net.Mail;
using Api.Services.Contracts;

namespace Api.Services.Implementations;

public class SmtpEmailService(IConfiguration config) : ISmtpEmailService
{
    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        var smtpSettings = config.GetSection("Smtp");

        var message = new MailMessage
        {
            From = new MailAddress(smtpSettings["From"]!, smtpSettings["SenderName"]!),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true,
        };

        message.To.Add(to);

        using var client = new SmtpClient
        {
            Host = smtpSettings["Host"]!,
            Port = int.Parse(smtpSettings["Port"]!),
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
        };

        await client.SendMailAsync(message);
    }
}
