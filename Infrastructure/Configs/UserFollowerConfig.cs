using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs;

public class UserFollowerConfig : IEntityTypeConfiguration<UserFollower>
{
    public void Configure(EntityTypeBuilder<UserFollower> builder)
    {
        // 1. Configure the "Following" side
        builder
            .HasOne(uf => uf.Follower)
            .WithMany(u => u.Followings) // Navigation property on User: "Who am I following?"
            .HasForeignKey(uf => uf.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        // 2. Configure the "Followers" side
        builder
            .HasOne(uf => uf.Following)
            .WithMany(u => u.Followers) // Navigation property on User: "Who follows me?"
            .HasForeignKey(uf => uf.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        // 3. Prevent a user from following themselves (Optional but recommended)
        // This requires a Check Constraint (SQL Server)
        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_UserFollower_SelfFollow",
                $"[{nameof(UserFollower.FollowerId)}] <> [{nameof(UserFollower.FollowingId)}]"
            )
        );

        // 4. Ensure a user can't follow the same person twice
        builder.HasIndex(uf => new { uf.FollowerId, uf.FollowingId }).IsUnique();
    }
}
