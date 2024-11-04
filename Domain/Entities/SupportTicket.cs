using Tickest.Domain.Enum;

namespace Tickest.Domain.Entities;

public class SupportTicket : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }

    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }

    public int RequesterId { get; set; }
    public User Requester { get; set; } // QUEM SOLICITA

    public int AttendantId { get; set; }
    public User Attendant { get; set; } //QUEM CONVERSA COM SOLICITANTE E DIRECIONA PARA O SETOR CORRETO

    public int? AnalystId { get; set; }
    public User Analyst { get; set; } //QUEM IRÁ RESOLVER DE FATO O PROBLEMA

    public int AreaId { get; set; }
    public Area Area { get; set; }

    public ICollection<Message> Messages { get; set; }
}
