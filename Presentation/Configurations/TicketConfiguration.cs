using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;
public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        //builder.HasKey(t => t.Id);
        //builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        //builder.Property(t => t.Description).HasMaxLength(1000);
        //builder.Property(t => t.CreatedDate).IsRequired();
        //builder.Property(t => t.CompletionDate).IsRequired(false); // Data de conclusão opcional
        //builder.HasOne(t => t.Requester)
        //       .WithMany(u => u.Tickets)
        //       .HasForeignKey(t => t.RequesterId);
        //builder.HasOne(t => t.Responsible)
        //       .WithMany(u => u.ResponsibleTickets)
        //       .HasForeignKey(t => t.ResponsibleId);
        //builder.HasMany(t => t.Messages)
        //       .WithOne(m => m.Ticket)
        //       .HasForeignKey(m => m.TicketId); // Relacionamento com Messages
    }
}