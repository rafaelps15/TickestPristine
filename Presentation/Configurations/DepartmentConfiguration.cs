using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("TB_DEPARTMENT");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasOne(s => s.ResponsibleUser)
               .WithMany()
               .HasForeignKey(s => s.ResponsibleUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Areas)
               .WithOne(a => a.Department)
               .HasForeignKey(a => a.DepartmentId);
    }
}
