namespace Tickest.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public string Salt { get; set; }
    public int? AreaId { get; set; }
    public DateTime DateRegistered { get; set; }
    public Area Area { get; set; }

    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();// Papéis (roles) do usuário
    public ICollection<SupportTicket> RequestedTickets { get; set; } // Chamados que o usuário abriu
    public ICollection<SupportTicket> AttendedTickets { get; set; } // Chamados atendidos pelo usuário
    public ICollection<SupportTicket> AnalystTickets { get; set; } // Chamados onde o usuário é analista
    public ICollection<Message> Messages { get; set; } // Mensagens enviadas pelo usuário

}
