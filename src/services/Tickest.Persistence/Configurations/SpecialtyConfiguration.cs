using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class SpecialtyConfiguration : BaseEntityConfiguration<Specialty>
{
    public override void Configure(EntityTypeBuilder<Specialty> builder)
    {
        base.Configure(builder);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Description).HasMaxLength(500);
    }
}

