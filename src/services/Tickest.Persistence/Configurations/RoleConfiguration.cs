using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);

        builder.Property(role => role.Name)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(role => role.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(role => role.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(role => role.Name)
            .IsUnique();
    }
}
