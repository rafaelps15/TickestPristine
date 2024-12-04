using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;

namespace Tickest.Infrastructure.Data.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("Areas");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Description)
            .HasMaxLength(255);

        builder.HasOne(a => a.Sector)
            .WithMany(s => s.Areas)
            .HasForeignKey(a => a.SectorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.ResponsibleUser)
            .WithMany()
            .HasForeignKey(a => a.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.AreaUserSpecialties)
            .WithOne()
            .HasForeignKey(aus => aus.AreaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
