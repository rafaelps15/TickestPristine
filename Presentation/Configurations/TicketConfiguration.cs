using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;
using Tickest.Domain.Enum;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        // Nome da tabela
        builder.ToTable("Tickets");

        // Chave primária
        builder.HasKey(t => t.Id);

        // Propriedades
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.CreatedDate)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<int>(); // Salva como int no banco de dados

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>(); // Salva como int no banco de dados

        builder.Property(t => t.IsDeleted)
            .HasDefaultValue(false);

        // Relacionamentos
        builder.HasOne(t => t.AssignedUser)
            .WithMany()
            .HasForeignKey(t => t.AssignedUserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false); // Permite valores nulos para AssignedUserId

        builder.HasOne(t => t.Requester)
            .WithMany()
            .HasForeignKey(t => t.RequesterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // Permite valores nulos para RequesterId

        builder.HasOne(t => t.Responsible)
            .WithMany()
            .HasForeignKey(t => t.ResponsibleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // Permite valores nulos para ResponsibleId

        builder.HasMany(t => t.Messages)
            .WithOne(m => m.Ticket)
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.TicketUsers)
            .WithOne(tu => tu.Ticket)
            .HasForeignKey(tu => tu.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
