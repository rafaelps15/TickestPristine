using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class SectorConfiguration : BaseEntityConfiguration<Sector>
{
    public override void Configure(EntityTypeBuilder<Sector> builder)
    {
        base.Configure(builder);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.HasOne(s => s.ResponsibleUser).WithMany().HasForeignKey(s => s.ResponsibleUserId).OnDelete(DeleteBehavior.Restrict);
    }
}

