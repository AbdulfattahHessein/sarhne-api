namespace Core.Entities;

public class UserFollower : BaseEntity
{
    public Guid FollowerId { get; set; }
    public User Follower { get; set; } = null!;

    public Guid FollowingId { get; set; }
    public User Following { get; set; } = null!;
}
