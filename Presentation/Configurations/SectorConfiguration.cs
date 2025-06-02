using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Sectors;

namespace Tickest.Persistence.Configurations;

public class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Description).HasMaxLength(1000);
        builder.HasOne(s => s.SectorManager).WithMany().HasForeignKey(s => s.SectorManagerId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(s => s.Departments).WithOne(d => d.Sector).HasForeignKey(d => d.SectorId).OnDelete(DeleteBehavior.NoAction);
        builder.ToTable("Sectors");
    }
}
