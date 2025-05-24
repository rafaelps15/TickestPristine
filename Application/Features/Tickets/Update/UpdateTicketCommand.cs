using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Enum;

namespace Tickest.Application.Features.Tickets.Update;

public record UpdateTicketCommand (
    Guid TicketId,
    string Description,
    TicketStatus Status) : ICommand<Guid>;
