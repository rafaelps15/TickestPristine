namespace Tickest.Domain.Entities
{
    #region User
    /// <summary>
    /// Representa um usuário no sistema, com informações de login, papéis e estado.
    /// Cada usuário pode estar associado a múltiplos tickets.
    /// </summary>
    public class User : EntityBase
    {
        #region Properties

        public string Name { get; set; } 
        public string Email { get; set; } 
        public string Password { get; set; } // Armazena o hash da senha.
        public string Salt { get; set; } // Armazena o salt gerado.
        public string Role { get; set; } // Ex: Admin, Analyst, Collaborator, etc.
        public ICollection<Ticket> Tickets { get; set; } // Coleção de tickets associados ao usuário.
        public DateTime DateRegistrationDate { get; set; } // Data de registro do usuário no sistema.
        public bool IsActive { get; set; } // Indica se o usuário está ativo ou não.

        #endregion
    }
    #endregion
}
