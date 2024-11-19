namespace Tickest.Domain.Entities;

/// <summary>
/// Representa um usuário no sistema, com informações de login, papéis e estado.
/// Cada usuário pode estar associado a múltiplos tickets.
/// </summary>
public class User : EntityBase
{
    #region Properties

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; }

    // Relacionamento obrigatório com uma especialidade principal
    public Guid MainSpecialtyId { get; set; }
    public Specialty MainSpecialty { get; set; }

    // Relacionamento N:N com especialidades adicionais
    public virtual ICollection<UserSpecialty> UserSpecialties { get; set; }

    // Relação de um para muitos com Ticket
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<TicketUser> TicketUsers { get; set; } // Relacionamento com a tabela de junção

    // Relação de um para muitos com UserRole
    public ICollection<UserRole> UserRoles { get; set; }

    // Relacionamento de um para muitos com as mensagens enviadas pelo usuário
    public ICollection<Message> Messages { get; set; }

    // Relacionamento de um para muitos com as respostas dadas pelo usuário (Awnser)
    public Guid? AnsweredId { get; set; } // Chave estrangeira para o usuário que enviou a mensagem
    public Message Answered { get; set; } // Navegação para a mensagem respondida (se houver) 

    // Propriedades de auditoria
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }


    #endregion
}
