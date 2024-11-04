using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

internal class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("TB_AREA");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(100);

        builder.HasOne(a => a.Department)
               .WithMany(s => s.Areas)
               .HasForeignKey(a => a.DepartmentId);

        builder.HasMany(a => a.Users)
               .WithOne(u => u.Area)
               .HasForeignKey(u => u.AreaId);

        builder.HasMany(a => a.Tickets)
               .WithOne(c => c.Area)
               .HasForeignKey(c => c.AreaId);
    }
}
