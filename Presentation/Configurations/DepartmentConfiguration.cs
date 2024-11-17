using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.HasMany(d => d.Areas)
               .WithOne(a => a.Department)
               .HasForeignKey(a => a.DepartmentId); // Relacionamento com Areas
        builder.HasMany(d => d.Sectors)
               .WithMany(s => s.Departments); // Relacionamento com Sectors
    }
}