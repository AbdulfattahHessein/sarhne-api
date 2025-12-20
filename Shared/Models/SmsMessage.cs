namespace Shared.Models;

public class SmsMessage(string message, params string[] phoneNumbers)
{
    public string Message { get; set; } = message;
    public HashSet<string> PhoneNumbers { get; set; } = [.. phoneNumbers];
}
