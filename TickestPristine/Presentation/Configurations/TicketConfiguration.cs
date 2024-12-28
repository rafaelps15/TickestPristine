using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Tickets;

namespace Tickest.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            // Configuração da chave primária
            builder.HasKey(t => t.Id);

            // Configuração das propriedades
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            // Relacionamento com o usuário que abriu o ticket (OpenedByUser)
            builder.HasOne(t => t.OpenedByUser)
                .WithMany() // Um usuário pode abrir muitos tickets
                .HasForeignKey(t => t.OpenedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Não excluir o ticket se o usuário for excluído

            // Relacionamento com o usuário ao qual o ticket foi atribuído (AssignedToUser)
            builder.HasOne(t => t.AssignedToUser)
                .WithMany() // Um usuário pode ser responsável por muitos tickets
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull); // Definir AssignedToUser como null se o usuário for excluído

            // Relacionamento com o departamento (Department)
            builder.HasOne(t => t.Department)
                .WithMany() // Um departamento pode ter muitos tickets
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); // Não excluir o ticket se o departamento for excluído

            // Relacionamento com o setor (Sector)
            builder.HasOne(t => t.Sector)
                .WithMany() // Um setor pode ter muitos tickets
                .HasForeignKey(t => t.SectorId)
                .OnDelete(DeleteBehavior.Restrict); // Não excluir o ticket se o setor for excluído

            // Relacionamento com a área (Area)
            builder.HasOne(t => t.Area)
                .WithMany() // Uma área pode ter muitos tickets
                .HasForeignKey(t => t.AreaId)
                .OnDelete(DeleteBehavior.Restrict); // Não excluir o ticket se a área for excluída

            // Configuração da tabela
            builder.ToTable("Tickets");
        }
    }
}
