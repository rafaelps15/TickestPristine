using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Enum;

namespace Tickest.Application.Tickets.Update;

public record UpdateTicketCommand (
    Guid TicketId,
    string Description,
    TicketStatus Status) : ICommand<Guid>;
