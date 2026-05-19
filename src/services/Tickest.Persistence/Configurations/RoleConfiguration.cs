using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Constants;
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

        builder.HasIndex(role => role.Name).IsUnique();
        builder.ToTable("Roles");

        builder.HasData(SystemRoles.All.Select(role => new Role
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }));
    }
}
