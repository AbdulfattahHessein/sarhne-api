using Microsoft.Extensions.Logging;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class DevEmailService(ILogger<DevEmailService> logger) : IEmailService
{
    private readonly ILogger<DevEmailService> _logger = logger;

    public async Task SendAsync(string to, string subject, string body)
    {
        await Task.Delay(100);

        _logger.LogInformation($"Email was sent to: [{to}]. Subject: [{subject}]. Body: {body}");
    }
}
