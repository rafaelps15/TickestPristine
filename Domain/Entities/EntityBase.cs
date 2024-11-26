namespace Tickest.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public bool IsActive { get; set; } = true; 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Data de criação da entidade
        public DateTime? UpdatedDate { get; set; } // Data da última atualização da entidade
        public DateTime? CompletionDate { get; set; } // Data de conclusão da entidade
        public DateTime? DeactivatedDate { get; set; } // Data de desativação da entidade
        public DateTime FirstRegistrationDate { get; set; } = DateTime.UtcNow; // Data do primeiro registro da entidade
    }
}
