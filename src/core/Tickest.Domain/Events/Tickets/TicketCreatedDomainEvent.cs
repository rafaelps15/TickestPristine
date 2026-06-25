using Tickest.SharedKernel;

namespace Tickest.Domain.Events.Tickets;

public sealed record TicketCreatedDomainEvent(
    Guid TicketId,
    Guid OpenedByUserId,
    Guid DepartmentId,
    Guid SectorId) : IDomainEvent;
