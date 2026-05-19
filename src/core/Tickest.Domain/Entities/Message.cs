using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Tickets;

#region Message
public class Message : EntityBase
{
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsEdited { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!; // Relacionamento com Ticket

    public int UserId { get; set; }
    public User User { get; set; } = null!; // Relacionamento com User

    public int? RepliedToMessageId { get; set; }
    public Message? RepliedToMessage { get; set; } // Relacionamento com Message (para mensagens respondidas)
}
#endregion
