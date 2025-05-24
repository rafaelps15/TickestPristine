using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Tickets.Delete;

public record SoftDeleteTicketCommand(
    Guid TicketId, Guid UserId) : ICommand<Guid>
{
}

