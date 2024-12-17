using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Tickets.Delete;

public  record SoftDeleteTicketCommand(
    Guid TicketId) : ICommand<Guid>;

