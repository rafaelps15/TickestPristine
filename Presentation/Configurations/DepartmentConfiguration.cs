using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Sectors;

namespace Tickest.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.HasOne(d => d.Sector).WithMany(s => s.Departments).HasForeignKey(d => d.SectorId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(d => d.DepartmentManager).WithMany().HasForeignKey(d => d.DepartmentManagerId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(d => d.Areas).WithOne(a => a.Department).HasForeignKey(a => a.DepartmentId);
        builder.ToTable("Departments");
    }
}
