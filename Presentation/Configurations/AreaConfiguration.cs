using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(500);
        builder.HasOne(a => a.Department).WithMany(d => d.Areas).HasForeignKey(a => a.DepartmentId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(a => a.AreaManager).WithMany().HasForeignKey(a => a.AreaManagerId).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(a => a.Users).WithMany(u => u.Areas).UsingEntity<Dictionary<string, object>>(
            "AreaUser",
            j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Area>().WithMany().HasForeignKey("AreaId").OnDelete(DeleteBehavior.Cascade));
    }
}  