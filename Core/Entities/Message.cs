using System;

namespace Core.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public bool IsPublic { get; set; }
    public bool IsFav { get; set; }
    public bool SendAnonymously { get; set; }
    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;
    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
}
