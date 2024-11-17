namespace Tickest.Domain.Entities;

#region Message
/// <summary>
/// Representa uma mensagem enviada por um usuário em um ticket (para chat).
/// </summary>
public class Message : EntityBase
{
    public string Content { get; set; }
    public DateTime SentDate { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; }
}
#endregion
