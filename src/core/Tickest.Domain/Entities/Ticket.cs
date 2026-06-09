using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Entities.Tickets;

#region Ticket
public class Ticket : EntityBase
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
}
#endregion
