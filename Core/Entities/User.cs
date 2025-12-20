using System;
using Core.Enums;

namespace Core.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfileSlug { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Gender Gender { get; set; }

    public ICollection<Message> SentMessages { get; set; } = [];
    public ICollection<Message> ReceivedMessages { get; set; } = [];

    public Settings Settings { get; set; }

    public User()
    {
        Settings = new Settings(Id);
    }

    public ICollection<UserFollower> Followers { get; set; } = []; //(People following me)

    public ICollection<UserFollower> Following { get; set; } = []; //(People I follow)
}
