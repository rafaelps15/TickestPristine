namespace Tickest.Domain.Entities;

public class TicketUser : EntityBase
{
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}
