using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Tickets;

namespace Tickest.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id); // Supondo que 'Id' é a chave primária

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.Priority)
            .IsRequired();

        // Configuração dos relacionamentos com User
        builder.HasOne(t => t.OpenedByUser)
            .WithMany() // Defina como Many caso queira que o usuário tenha uma coleção de Tickets
            .HasForeignKey(t => t.OpenedByUserId)
            .OnDelete(DeleteBehavior.NoAction) // Impede exclusões automáticas
            .IsRequired(false);  // Permite nulos

        builder.HasOne(t => t.AssignedToUser)
            .WithMany() // Defina como Many caso queira que o usuário tenha uma coleção de Tickets
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.NoAction) // Impede exclusões automáticas
            .IsRequired(false);  // Permite nulos

        // Relacionamento com o Departamento, Setor e Área
        builder.HasOne(t => t.Department)
            .WithMany()
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);  

        builder.HasOne(t => t.Sector)
            .WithMany()
            .HasForeignKey(t => t.SectorId)
            .OnDelete(DeleteBehavior.Cascade);  

        builder.HasOne(t => t.Area)
            .WithMany()
            .HasForeignKey(t => t.AreaId)
            .OnDelete(DeleteBehavior.Cascade);  

        // Relacionamento com as mensagens
        builder.HasMany(t => t.Messages)
            .WithOne(m => m.Ticket)
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
}
