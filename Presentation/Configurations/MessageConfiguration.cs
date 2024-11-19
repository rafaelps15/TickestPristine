using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Tabela (se necessário)
        builder.ToTable("Messages");

        // Configurações de propriedades
        builder.Property(m => m.Content)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(1000); // Limite de 1000 caracteres para o conteúdo da mensagem

        builder.Property(m => m.SentDate)
            .IsRequired(); // Data de envio obrigatória

        // Relacionamentos
        // Relacionamento com Ticket (Muitos para 1)
        builder.HasOne(m => m.Ticket)
            .WithMany(t => t.Messages) // Um ticket pode ter várias mensagens
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.Cascade); // Exclua as mensagens ao excluir o ticket

        // Relacionamento com User (Muitos para 1)
        builder.HasOne(m => m.User)
            .WithMany(u => u.Messages) // Um usuário pode enviar várias mensagens
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Exclua as mensagens ao excluir o usuário

        // Relacionamento com a mensagem respondida (auto-relacionamento, Muitos para 1)
        builder.HasOne(m => m.Answered) // A mensagem que está sendo respondida
            .WithMany() // Uma mensagem pode ter várias respostas, mas não precisa de navegação reversa
            .HasForeignKey(m => m.AnsweredId)
            .OnDelete(DeleteBehavior.Restrict); // Evite exclusão em cascata para preservar respostas antigas

        // Relacionamento com usuários que responderam (Muitos para muitos)
        builder.HasMany(m => m.UsersWhoAnswered) // Propriedade de navegação no Message
            .WithOne(u => u.Answered) // Propriedade de navegação no User
            .HasForeignKey(u => u.AnsweredId); // Chave estrangeira
    }
}
