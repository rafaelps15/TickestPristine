using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class PermissionConfiguration : BaseEntityConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(200);
    }
}

