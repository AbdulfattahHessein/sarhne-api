using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Shared.Models;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class SmsService(IConfiguration config) : ISmsService
{
    public async Task<string> SendSmsMessage(SmsMessage sms)
    {
        var url = config["Sms:SMS_URL"];

        if (string.IsNullOrEmpty(url))
            throw new InvalidOperationException("SMS_URL Is Null Or Empty");

        url = url.Replace("{MSG_CONTENT}", sms.Message);
        url = url.Replace("{SMS_PHONE_LIST}", string.Join(",", sms.PhoneNumbers));
        using var client = new HttpClient();

        client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));

        try
        {
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Throws an exception if the status code is not successful

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to send SMS message", ex);
        }
    }
}
