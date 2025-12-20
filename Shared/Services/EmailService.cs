using MailKit.Net.Smtp;
using MimeKit;
using Shared.Models.Email;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class EmailService(SmtpSettings smtpSettings) : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await SendAsync(CreateEmail(emailMessage));
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);

            await client.SendAsync(message);
        }
        catch
        {
            await client.DisconnectAsync(true);
            client.Dispose();

            throw;
        }
    }

    private MimeMessage CreateEmail(EmailMessage emailMessage)
    {
        var builder = new BodyBuilder { HtmlBody = emailMessage.Body };

        emailMessage.Attachments.ForEach(attachment =>
            builder.Attachments.Add(attachment.Name, attachment.Value)
        );

        var from = new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail);

        var to = new MailboxAddress(emailMessage.ToAddress.Split("@")[0], emailMessage.ToAddress);

        var email = new MimeMessage([from], [to], emailMessage.Subject, builder.ToMessageBody());

        return email;
    }
}
