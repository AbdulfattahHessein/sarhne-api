using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Models.Email;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class DevEmailService(ILogger<DevEmailService> logger) : IEmailService
{
    private readonly ILogger<DevEmailService> _logger = logger;

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);

        _logger.LogInformation(
            $"Email was sent to: [{emailMessage.ToAddress}]. Body: {emailMessage.Body}"
        );
    }
}

public class DevSmsService(ILogger<DevEmailService> logger) : ISmsService
{
    private readonly ILogger<DevEmailService> _logger = logger;

    public async Task<string> SendSmsMessage(SmsMessage sms)
    {
        await Task.Delay(100);

        _logger.LogInformation(
            "SMS was sent to: [{phoneNumbers}]. Body: {Message}",
            string.Join(',', sms.PhoneNumbers),
            sms.Message
        );

        return "Sms was sent successfully";
    }
}
