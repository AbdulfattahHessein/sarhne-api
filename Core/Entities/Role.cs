using Core.Enums;

namespace Core.Entities;

public class Role : BaseEntity
{
    public RoleType Name { get; set; }
    public ICollection<User> Users { get; set; } = [];
}
