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

    /// <summary>
    /// Relacionamento obrigatório com uma especialidade principal
    /// </summary>
    public Guid MainSpecialtyId { get; set; }
    public Specialty MainSpecialty { get; set; }

    /// <summary>
    /// Relacionamento N:N com especialidades adicionais
    /// </summary>
    public virtual ICollection<UserSpecialty> UserSpecialties { get; set; }

    /// <summary>
    /// Relação de um para muitos com Ticket
    /// </summary>
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<TicketUser> TicketUsers { get; set; } // Relacionamento com a tabela de junção

    /// <summary>
    /// Relação de um para muitos com UserRole
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; }

    /// <summary>
    /// Relacionamento de um para muitos com as mensagens enviadas pelo usuário
    /// </summary>
    public ICollection<Message> Messages { get; set; }

    /// <summary>
    /// Relacionamento de um para muitos com as respostas dadas pelo usuário (Awnser)
    /// </summary>
    public Guid? AnsweredId { get; set; } // Chave estrangeira para o usuário que enviou a mensagem
    public Message Answered { get; set; } // Navegação para a mensagem respondida (se houver) 

    /// <summary>
    /// Propriedades de auditoria
    /// </summary>
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public Guid? RefreshTokenId { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }

    #endregion
}
