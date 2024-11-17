using Tickest.Domain.Enum;

namespace Tickest.Domain.Entities;

#region Ticket
/// <summary>
/// Representa um ticket de solicitação ou suporte no sistema.
/// Cada ticket contém informações sobre sua criação, status, prioridade, e os usuários envolvidos.
/// </summary>
public class Ticket : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Registra a data de criação automaticamente
    public DateTime? CompletionDate { get; set; } // Data de conclusão do ticket
    public DateTime? DeactivatedDate { get; set; }// Data de desativação do ticket
    public bool IsActive { get; set; } = true; // Define se o ticket está ativo ou não

    public TicketPriority Priority { get; set; } 
    public TicketStatus Status { get; set; }  

    public Guid RequesterId { get; set; }
    public User Requester { get; set; }
    public Guid ResponsibleId { get; set; }
    public User Responsible { get; set; }

    public ICollection<Message> Messages { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
}
#endregion