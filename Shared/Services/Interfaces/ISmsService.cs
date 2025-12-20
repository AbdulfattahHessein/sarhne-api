using Shared.Models;

namespace Shared.Services.Interfaces;

public interface ISmsService
{
    public Task<string> SendSmsMessage(SmsMessage sms);
}
