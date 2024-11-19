using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class TicketUserConfiguration : IEntityTypeConfiguration<TicketUser>
{
    public void Configure(EntityTypeBuilder<TicketUser> builder)
    {
        // Nome da tabela
        builder.ToTable("TicketUsers");

        // Chave composta
        builder.HasKey(tu => new { tu.TicketId, tu.UserId });

        // Relacionamentos
        builder.HasOne(tu => tu.Ticket)
            .WithMany(t => t.TicketUsers)
            .HasForeignKey(tu => tu.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tu => tu.User)
            .WithMany(u => u.TicketUsers)
            .HasForeignKey(tu => tu.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
