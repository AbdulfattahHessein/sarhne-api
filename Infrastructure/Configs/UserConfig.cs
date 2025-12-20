using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Gender).HasConversion<string>();

        builder.HasOne(u => u.Settings).WithOne().HasForeignKey<Settings>(s => s.Id);
    }
}
