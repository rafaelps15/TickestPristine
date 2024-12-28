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
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketPriority Priority { get; set; }
        public TicketStatus Status { get; set; }
        public string OpenedBy { get; set; } // Nome de quem abriu
        public string AssignedTo { get; set; } // Nome de quem foi atribuído
        public string Department { get; set; } // Nome do departamento
        public string Sector { get; set; } // Nome do setor
        public string Area { get; set; } // Nome da área
    }
}
