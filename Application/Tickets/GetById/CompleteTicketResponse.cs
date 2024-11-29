using Tickest.Domain.Enum;

namespace Tickest.Application.Tickets.GetById;

public record CompleteTicketResponse(Guid Id, string Title, TicketStatus Status, string Description);


