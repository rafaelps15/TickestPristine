using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;

namespace Tickest.Application.Tickets.GetById;

public class GetByIdUserTicketsQuery : IQuery<Result<IEnumerable<CompleteTicketResponse>>>
{
    public Guid UserId { get; set; }
}
