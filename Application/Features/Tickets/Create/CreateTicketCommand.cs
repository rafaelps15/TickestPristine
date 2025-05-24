using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Enum;

namespace Tickest.Application.Features.Tickets.Create;

public record CreateTicketCommand(
     string Title,
     string Description,
     TicketPriority Priority,
     Guid? RequesterId,
     Guid ResponsibleId) : ICommand<Guid>;

// RequesterId -> Opcional, se um admin criar para outro usuário



