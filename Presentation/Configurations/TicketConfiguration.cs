using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

internal class TicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        // Nome da tabela
        builder.ToTable("TB_TICKET");

        // Configuração da chave primária
        builder.HasKey(c => c.Id);

        // Configuração das propriedades
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.OpeningDate)
            .IsRequired();

        builder.Property(c => c.ClosingDate)
            .IsRequired(false);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.Priority)
            .IsRequired();

        // Configuração das relações
        builder.HasOne(c => c.Requester)
            .WithMany(u => u.RequestedTickets)
            .HasForeignKey(c => c.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Attendant)
            .WithMany(u => u.AttendedTickets)
            .HasForeignKey(c => c.AttendantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Analyst)
            .WithMany(u => u.AnalystTickets)
            .HasForeignKey(c => c.AnalystId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Area)
            .WithMany(a => a.Tickets)
            .HasForeignKey(c => c.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Messages)
            .WithOne(m => m.SupportTicket)
            .HasForeignKey(m => m.SupportTicketId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
