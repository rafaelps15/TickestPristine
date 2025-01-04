using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Tickets;

namespace Tickest.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // Define a chave primária
        builder.HasKey(m => m.Id);

        // Define a tabela
        builder.ToTable("Messages");

        // Define o relacionamento com o Sender (Remetente)
        builder.HasOne(m => m.Sender)
            .WithMany()  // Relacionamento 1:N (um usuário pode ter várias mensagens enviadas)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict); // Evita cascata ou ciclo (NO ACTION ou RESTRICT)

        // Define o relacionamento com o Receiver (Destinatário)
        builder.HasOne(m => m.Receiver)
            .WithMany()  // Relacionamento 1:N (um usuário pode ter várias mensagens recebidas)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict); // Definindo a ação de deleção como SetNull

        // Define o relacionamento com o Ticket
        builder.HasOne(m => m.Ticket)
            .WithMany()  // Relacionamento 1:N (um ticket pode ter várias mensagens)
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.Cascade); // Se o ticket for excluído, as mensagens também são excluídas

        // Define os tamanhos máximos das propriedades
        builder.Property(m => m.Content)
            .IsRequired() // Define que o conteúdo da mensagem é obrigatório
            .HasMaxLength(1000); // Define o tamanho máximo do conteúdo

        builder.Property(m => m.SentDate)
            .IsRequired(); // Define que a data de envio é obrigatória
    }
}
