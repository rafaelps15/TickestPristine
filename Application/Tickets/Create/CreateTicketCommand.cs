using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Enum;

namespace Tickest.Application.Tickets.Create;

public class CreateTicketCommand : ICommand<Ticket>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketPriority Priority { get; set; }
    public Guid RequesterId { get; set; }
    public Guid ResponsibleId { get; set; }
}
