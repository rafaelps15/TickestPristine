using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Tickets;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class TicketConfiguration : BaseEntityConfiguration<Ticket>
{
    public override void Configure(EntityTypeBuilder<Ticket> builder)
    {
        base.Configure(builder);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.HasOne(t => t.OpenedByUser).WithMany().HasForeignKey(t => t.OpenedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.AssignedToUser).WithMany().HasForeignKey(t => t.AssignedToUserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(t => t.Department).WithMany().HasForeignKey(t => t.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(t => t.Sector).WithMany().HasForeignKey(t => t.SectorId).OnDelete(DeleteBehavior.Restrict);
    }
}

