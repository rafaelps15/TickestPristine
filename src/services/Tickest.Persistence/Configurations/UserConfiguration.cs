using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
        builder.Property(u => u.EmployeeCode).IsRequired().HasMaxLength(10);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.EmployeeCode).IsUnique();
        builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(u => u.Sector).WithMany(s => s.Users).HasForeignKey(u => u.SectorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.UserSpecialties).WithOne(us => us.User).HasForeignKey(us => us.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.Permissions)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "UserPermissions",
                j => j.HasOne<Permission>().WithMany().HasForeignKey("PermissionId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));
    }
}

