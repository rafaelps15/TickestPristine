using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(m => m.Id); // Supondo que 'Id' é a chave primária

        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(1000); // Defina o comprimento conforme necessário

        builder.Property(m => m.SentDate)
            .IsRequired();

        // Configuração do relacionamento com o Ticket
        builder.HasOne(m => m.Ticket)
            .WithMany(t => t.Messages)
            .HasForeignKey(m => m.TicketId)
            .OnDelete(DeleteBehavior.Cascade); // Se o ticket for excluído, as mensagens serão excluídas

        // Relacionamento com o Sender
        builder.HasOne(m => m.Sender)
            .WithMany() // Caso o Sender não precise de uma coleção de mensagens
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.NoAction); // Não excluir o Sender quando a mensagem for excluída

        // Relacionamento com o Receiver
        builder.HasOne(m => m.Receiver)
            .WithMany() // Caso o Receiver não precise de uma coleção de mensagens
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.NoAction); // Não excluir o Receiver quando a mensagem for excluída
    }
}
