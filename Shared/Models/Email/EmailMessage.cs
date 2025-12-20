namespace Shared.Models.Email;

public class EmailMessage
{
    private EmailMessage(
        string toAddress,
        string body,
        string subject,
        List<EmailAttachment>? attachments = null
    )
    {
        ToAddress = toAddress;
        Body = body;
        Subject = subject;
        Attachments = attachments ?? [];
    }

    public string ToAddress { get; private set; }

    public string Body { get; private set; }

    public string Subject { get; private set; }

    public List<EmailAttachment> Attachments { get; private set; }

    public static EmailMessage Create(
        string toAddress,
        string body,
        string subject,
        List<EmailAttachment>? attachments = null
    )
    {
        return new EmailMessage(toAddress, body, subject, attachments);
    }

    public static EmailMessage Create(
        string toAddress,
        string body,
        string subject,
        EmailAttachment attachment
    )
    {
        return Create(toAddress, body, subject, [attachment]);
    }
}
