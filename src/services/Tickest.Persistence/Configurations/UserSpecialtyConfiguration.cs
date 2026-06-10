using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Persistence.Configurations;

public class UserSpecialtyConfiguration : IEntityTypeConfiguration<UserSpecialty>
{
    public void Configure(EntityTypeBuilder<UserSpecialty> builder)
    {
        builder.HasKey(us => new { us.UserId, us.SpecialtyId });
        builder.HasOne(us => us.User).WithMany(u => u.UserSpecialties).HasForeignKey(us => us.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(us => us.Specialty).WithMany(s => s.UserSpecialties).HasForeignKey(us => us.SpecialtyId).OnDelete(DeleteBehavior.Restrict);
    }
}

