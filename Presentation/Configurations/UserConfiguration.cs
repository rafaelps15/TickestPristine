using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Definir o nome da tabela
        builder.ToTable("Users");

        // Configurações de chave primária
        builder.HasKey(u => u.Id);

        // Configuração das propriedades
        builder.Property(u => u.Name)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(100); // Limite de 100 caracteres

        builder.Property(u => u.Email)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(150); // Limite de 150 caracteres

        builder.Property(u => u.Password)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(200); // Limite de 200 caracteres

        // Configuração de relacionamentos

        // Relacionamento com TicketUser (Muitos para Muitos)
        builder.HasMany(u => u.TicketUsers)
            .WithOne(tu => tu.User) // Cada TicketUser tem um User
            .HasForeignKey(tu => tu.UserId) // Chave estrangeira do User
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata ao excluir o usuário

        // Relacionamento com Message (Muitos para Muitos, via Answered)
        builder.HasOne(u => u.Answered) // Relacionamento com a mensagem respondida
            .WithMany(m => m.UsersWhoAnswered) // Coleção de usuários que responderam
            .HasForeignKey(u => u.AnsweredId) // Chave estrangeira que aponta para a mensagem respondida
            .OnDelete(DeleteBehavior.Restrict); // Impede exclusão em cascata acidental
    }
}
