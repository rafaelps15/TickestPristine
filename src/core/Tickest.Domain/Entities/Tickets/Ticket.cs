using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Enum;
using Tickest.Domain.Events.Tickets;
using Tickest.SharedKernel;

namespace Tickest.Domain.Entities.Tickets;

public class Ticket : AuditableEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public TicketPriority Priority { get; private set; }
    public TicketStatus Status { get; private set; }

    public EntityId OpenedByUserId { get; private set; } = null!;
    public User OpenedByUser { get; private set; } = null!;

    public EntityId? AssignedToUserId { get; private set; }
    public User? AssignedToUser { get; private set; }

    public EntityId DepartmentId { get; private set; } = null!;
    public Department Department { get; private set; } = null!;

    public EntityId SectorId { get; private set; } = null!;
    public Sector Sector { get; private set; } = null!;

    private Ticket()
    {
    }

    public static Ticket Create(
        string title,
        string description,
        TicketPriority priority,
        Guid openedByUserId,
        Guid? assignedToUserId,
        Guid departmentId,
        Guid sectorId)
    {
        var ticket = new Ticket
        {
            Title = title,
            Description = description,
            Priority = priority,
            Status = TicketStatus.Open,
            OpenedByUserId = openedByUserId,
            AssignedToUserId = assignedToUserId,
            DepartmentId = departmentId,
            SectorId = sectorId
        };

        ticket.RaiseDomainEvent(new TicketCreatedDomainEvent(
            ticket.Id,
            ticket.OpenedByUserId,
            ticket.DepartmentId,
            ticket.SectorId));

        return ticket;
    }

    public void Update(string description, TicketStatus status)
    {
        Description = description;
        Status = status;
    }

    public void Reopen(DateTime utcNow)
    {
        Activate(utcNow);
        Status = TicketStatus.Open;
    }
}
