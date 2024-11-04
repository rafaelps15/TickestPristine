using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("TB_USER");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(100);

        builder.HasOne(p => p.Area)
               .WithMany(p => p.Users)
               .HasForeignKey(p => p.AreaId);


        builder.HasMany(u => u.UserRules)
               .WithOne(ur => ur.User)
               .HasForeignKey(ur => ur.UserId);

        builder.HasMany(u => u.RequestedTickets)
               .WithOne(c => c.Requester)
               .HasForeignKey(c => c.RequesterId);

        builder.HasMany(u => u.AttendedTickets)
               .WithOne(c => c.Attendant)
               .HasForeignKey(c => c.AttendantId);

        builder.HasMany(u => u.Messages)
               .WithOne(m => m.User)
               .HasForeignKey(m => m.UserId);
    }
}
