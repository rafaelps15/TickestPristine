using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Enum;

namespace Tickest.Domain.Entities.Tickets;

#region Ticket
/// <summary>
/// Ticket: Representa um ticket dentro do sistema, com informações sobre o problema ou solicitação.
/// </summary>
public class Ticket : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }


    // Relacionamento com o usuário que abriu o ticket
    public Guid OpenedByUserId { get; set; }
    public User OpenedByUser { get; set; } // Usuário que abriu o ticket (ex: Cliente ou Colaborador)


    // Relacionamento do usuário a quem o ticket foi atribuído
    public Guid? AssignedToUserId { get; set; }
    public User AssignedToUser { get; set; } // Usuário atribuído ao ticket (ex: Analista ou Responsável TI)


    // Relacionamento com o departamento ao qual o ticket está associado
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } // Departamento (ex: TI, Suporte, etc.)


    // Relacionamento com o setor ao qual o ticket pertence
    public Guid SectorId { get; set; }
    public Sector Sector { get; set; } // Setor (ex: Desenvolvimento, Infraestrutura, etc.)


    // Relacionamento com a área do setor onde o ticket está alocado
    public Guid AreaId { get; set; }
    public Area Area { get; set; } // Área (ex: Frontend, Backend, etc.)

    // Relacionamento com as mensagens
    public List<Message> Messages { get; set; }  
}

#endregion
