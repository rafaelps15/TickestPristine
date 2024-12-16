using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;

namespace Tickest.Application.Tickets.GetById;

public sealed record GetByIdUserTicketsQuery : IQuery<Result<IEnumerable<CompleteTicketResponse>>>
{
    public Guid UserId { get; set; }
}
