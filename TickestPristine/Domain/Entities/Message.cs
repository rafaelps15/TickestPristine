using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Tickets;

#region Message
/// <summary>
/// Message: Representa uma mensagem enviada por um usuário em um ticket (para chat).
/// </summary>
public class Message : EntityBase
{
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsEdited { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } // Relacionamento com Ticket

    public int UserId { get; set; }
    public User User { get; set; } // Relacionamento com User

    public int? RepliedToMessageId { get; set; }
    public Message RepliedToMessage { get; set; } // Relacionamento com Message (para mensagens respondidas)
}
#endregion
