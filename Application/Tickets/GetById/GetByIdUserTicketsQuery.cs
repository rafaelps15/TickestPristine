using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Tickets.GetById;

public sealed record GetByIdUserTicketsQuery(Guid UserId) : IQuery<List<TicketResponse>>;
