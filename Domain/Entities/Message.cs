namespace Tickest.Domain.Entities;

public class Message : EntityBase
{
    public string Content { get; set; }
    public DateTime SentDate { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int SupportTicketId { get; set; }
    public SupportTicket SupportTicket { get; set; }
    
}
