using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Persistence.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(500);
        builder.HasOne(s => s.Area).WithMany(a => a.Specialties).HasForeignKey(s => s.AreaId).OnDelete(DeleteBehavior.Restrict);
        builder.Property(s => s.Description).IsRequired(false).HasMaxLength(500);
        builder.HasMany(s => s.Users).WithMany(u => u.Specialties).UsingEntity(j => j.ToTable("UserSpecialties"));
        builder.ToTable("Specialties");
    }
}
