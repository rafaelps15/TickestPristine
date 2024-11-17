using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        //builder.HasKey(m => m.Id);
        //builder.Property(m => m.Content).IsRequired();
        //builder.Property(m => m.SentDate).IsRequired();
        //builder.HasOne(m => m.User)
        //       .WithMany(u => u.Messages)
        //       .HasForeignKey(m => m.UserId);
        //builder.HasOne(m => m.Ticket)
        //       .WithMany(t => t.Messages)
        //       .HasForeignKey(m => m.TicketId); // Relacionamento com User e Ticket
    }
}