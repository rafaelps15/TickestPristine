using Tickest.Domain.Enum;

namespace Tickest.Domain.Entities;

/// <summary>
/// Representa um ticket de solicitação ou suporte no sistema.
/// Cada ticket contém informações sobre sua criação, status, prioridade, e os usuários envolvidos.
/// </summary>
public class Ticket : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
 
    // Relacionamentos
    public Guid? AssignedUserId { get; set; }
    public User AssignedUser { get; set; }

    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }

    public Guid RequesterId { get; set; }
    public User Requester { get; set; }

    public Guid ResponsibleId { get; set; }
    public User Responsible { get; set; }

    public ICollection<User> Users { get; set; }  // Relacionamento N:N com User
    public ICollection<TicketUser> TicketUsers { get; set; } // Relacionamento com a tabela de junção
    public ICollection<Message> Messages { get; set; }  // Relacionamento com mensagens

    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
}
