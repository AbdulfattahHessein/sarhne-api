using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Gender).HasConversion<string>();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasOne(u => u.Settings).WithOne().HasForeignKey<Settings>(s => s.Id);

        builder.Property(u => u.SecurityStamp).HasDefaultValueSql("NEWSEQUENTIALID()");

        ConfigureUserRoles(builder);
    }

    private static void ConfigureUserRoles(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                // Reference the Role navigation and its Foreign Key
                l => l.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId),
                // Reference the User navigation and its Foreign Key
                r => r.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId),
                j =>
                {
                    // Keep your specific PK
                    j.HasKey(ur => ur.Id);

                    // Ensure the unique constraint so a user isn't assigned
                    // the same role multiple times
                    j.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
                }
            );
    }
}
