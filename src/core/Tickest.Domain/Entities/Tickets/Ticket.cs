using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Enum;
using Tickest.Domain.Events.Tickets;

namespace Tickest.Domain.Entities.Tickets;

public class Ticket : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }

    public Guid OpenedByUserId { get; set; }
    public User OpenedByUser { get; set; } = null!;

    public Guid? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public Guid SectorId { get; set; }
    public Sector Sector { get; set; } = null!;

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
}
