namespace Tickest.Domain.Entities;

#region Message

/// <summary>
/// Representa uma mensagem enviada por um usuário em um ticket (para chat).
/// </summary>
public class Message : EntityBase
{
    public string Content { get; set; }
    public Guid TicketId { get; set; }
    public Ticket Ticket { get; set; }

    // Relacionamento com o usuário que enviou a mensagem
    public Guid UserId { get; set; } // Chave estrangeira do usuário que enviou a mensagem
    public User User { get; set; } // Navegação para o usuário que enviou a mensagem

    // Relacionamento com a mensagem respondida
    public Guid? AnsweredId { get; set; } // Pode ser nulo, pois nem toda mensagem é uma resposta
    public Message Answered { get; set; } // Navegação para a mensagem respondida (auto-relacionamento)

    // Lista de usuários que responderam a esta mensagem
    public ICollection<User> UsersWhoAnswered { get; set; } = new List<User>();
}

#endregion
