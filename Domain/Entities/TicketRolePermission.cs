namespace Tickest.Domain.Entities;

public class TicketRolePermission : EntityBase
{
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public string Role { get; set; }  // Role como 'TicketManager', 'Collaborator', etc.
    public bool CanSendMessage { get; set; }  // Indica se a role pode enviar mensagens
    public bool CanViewMessage { get; set; }  // Indica se a role pode visualizar mensagens
}
