using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Salt).IsRequired();

        builder.HasMany(u => u.Specialties).WithMany(s => s.Users).UsingEntity(j => j.ToTable("UserSpecialties"));
        builder.HasOne(u => u.Sector).WithMany().HasForeignKey(u => u.SectorId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(u => u.Areas).WithMany(a => a.Users).UsingEntity(j => j.ToTable("UserAreas"));
        builder.HasMany(u => u.Messages).WithOne(m => m.Sender).HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.SetNull);
    }
}
