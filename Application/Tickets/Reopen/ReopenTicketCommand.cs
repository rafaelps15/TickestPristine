using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.Reopen;

namespace Tickest.Application.Tickets.Reopen;

public class ReopenTicketCommand : ICommand<ReopenTicketResponse>
{
    public Guid TicketId { get; set; }
}
