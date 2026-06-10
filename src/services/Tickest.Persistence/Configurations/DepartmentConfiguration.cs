using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class DepartmentConfiguration : BaseEntityConfiguration<Department>
{
    public override void Configure(EntityTypeBuilder<Department> builder)
    {
        base.Configure(builder);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.HasMany(d => d.Sectors).WithOne(s => s.Department).HasForeignKey(s => s.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.ResponsibleUser).WithMany().HasForeignKey(d => d.ResponsibleUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

