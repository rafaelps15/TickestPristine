using Tickest.Domain.Enum;

namespace Tickest.Application.Tickets.GetById;

public sealed class CompleteTicketResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TicketStatus Status { get; set; }
    public string Description { get; set; }
}

