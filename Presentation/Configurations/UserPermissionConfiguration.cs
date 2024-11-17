using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;
public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        //builder.HasKey(up => up.Id);
        //builder.HasOne(up => up.User)
        //       .WithMany(u => u.UserPermissions)
        //       .HasForeignKey(up => up.UserId);
        //builder.HasOne(up => up.Permission)
        //       .WithMany(p => p.UserPermissions)
        //       .HasForeignKey(up => up.PermissionId); // Relacionamento entre User e Permission
    }
}