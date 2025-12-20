namespace Shared.Models.Email;

public class EmailAttachment
{
    public byte[] Value { get; private set; }

    public string Name { get; private set; }

    private EmailAttachment(string name, byte[] value)
    {
        Name = name;
        Value = value;
    }

    public static EmailAttachment Create(byte[] value, string name)
    {
        return new EmailAttachment(name, value);
    }
}
