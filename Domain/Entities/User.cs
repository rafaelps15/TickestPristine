namespace Tickest.Domain.Entities
{
    public class User : EntityBase
    {
        #region Properties

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

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
        /// Relacionamento de um para muitos com as respostas dadas pelo usuário (Answer)
        /// </summary>
        public Guid? AnsweredId { get; set; } // Chave estrangeira para o usuário que enviou a mensagem
        public Message Answered { get; set; } // Navegação para a mensagem respondida (se houver)

        //public Guid? RefreshTokenId { get; set; }
        //public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<Permission> Permissions { get; private set; }

        /// <summary>
        /// Relacionamento N:N entre usuários e permissões específicas
        /// </summary>
        public ICollection<UserPermission> UserPermissions { get; set; } = new HashSet<UserPermission>();

        #region Setor, Departamento e Área

        /// <summary>
        /// Relacionamento com Setor
        /// </summary>
        public Guid? SectorId { get; set; }
        public Sector UserSector { get; set; }

        /// <summary>
        /// Relacionamento com Departamento
        /// </summary>
        public Guid? DepartmentId { get; set; }
        public Department UserDepartment { get; set; }

        /// <summary>
        /// Relacionamento com Área
        /// </summary>
        public Guid? AreaId { get; set; }
        public Area UserArea { get; set; }

        #endregion

        #endregion
    }
}
