using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Tickets;

namespace Tickest.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        // Configuração das propriedades
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200); // Limita o título do ticket a 200 caracteres

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(1000); // Limita a descrição a 1000 caracteres

        builder.Property(t => t.Priority)
            .IsRequired(); // A prioridade é obrigatória

        builder.Property(t => t.Status)
            .IsRequired(); // O status é obrigatório

        // Relacionamento com o usuário que abriu o ticket
        builder.HasOne(t => t.OpenedByUser)
            .WithMany()
            .HasForeignKey(t => t.OpenedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Restrição de deletação

        // Relacionamento com o usuário atribuído ao ticket
        builder.HasOne(t => t.AssignedToUser)
            .WithMany()
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull); // Deleta o relacionamento, mas não o usuário

        // Relacionamento com o departamento
        builder.HasOne(t => t.Department)
            .WithMany()
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict); // Restrição de deletação

        // Relacionamento com o setor
        builder.HasOne(t => t.Sector)
            .WithMany()
            .HasForeignKey(t => t.SectorId)
            .OnDelete(DeleteBehavior.Restrict); // Restrição de deletação

        // Relacionamento com a área
        builder.HasOne(t => t.Area)
            .WithMany()
            .HasForeignKey(t => t.AreaId)
            .OnDelete(DeleteBehavior.Restrict); // Restrição de deletação

        // Configuração de tabela
        builder.ToTable("Tickets");
    }
}
