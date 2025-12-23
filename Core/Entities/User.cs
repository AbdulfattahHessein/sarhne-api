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
    public string PasswordHash
    {
        get;
        set
        {
            field = value;
            SecurityStamp = Guid.NewGuid();
        }
    } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public bool IsActive { get; set; }
    public Gender Gender { get; set; }
    public Guid SecurityStamp { get; set; }
    public ICollection<Message> SentMessages { get; set; } = [];
    public ICollection<Message> ReceivedMessages { get; set; } = [];

    public Settings Settings { get; set; }

    public User()
    {
        Settings = new Settings(Id);
    }

    public ICollection<UserFollower> Followers { get; set; } = []; //(People following me)

    public ICollection<UserFollower> Followings { get; set; } = []; //(People I follow)
    public ICollection<Role> Roles { get; set; } = [];
}
