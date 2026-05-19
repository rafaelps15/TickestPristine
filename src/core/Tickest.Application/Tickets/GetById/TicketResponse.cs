using Tickest.Domain.Enum;

namespace Tickest.Application.Tickets.GetById
{
    public sealed class TicketResponse
    {
        //public TicketResponse(Guid id, string title, TicketStatus status, string description)
        //{
        //    Id = id;
        //    Title = title;
        //    Status = status;
        //    Description = description;
        //}

        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public string OpenedBy { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
    }
}
