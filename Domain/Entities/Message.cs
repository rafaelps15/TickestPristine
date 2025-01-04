using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Tickets;

#region Message
/// <summary>
/// Message: Representa uma mensagem enviada por um usuário em um ticket (para chat).
/// </summary>
public class Message : EntityBase
{
    public string Content { get; set; } // Conteúdo da mensagem
    public DateTime SentDate { get; set; } // Data e hora do envio

    public Guid? SenderId { get; set; }
    public User Sender { get; set; }

    public Guid? ReceiverId { get; set; }
    public User Receiver { get; set; }

    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; }
}
#endregion
