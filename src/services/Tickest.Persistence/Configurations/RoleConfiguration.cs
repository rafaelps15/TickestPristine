using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(80);
        builder.Property(r => r.Description).IsRequired().HasMaxLength(200);
        builder.HasIndex(r => r.Name).IsUnique();
    }
}

