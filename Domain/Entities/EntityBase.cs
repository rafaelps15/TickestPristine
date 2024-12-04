namespace Tickest.Domain.Entities.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }  
        public bool IsActive { get; set; }  
        public bool IsDeleted { get; private set; }   // Indica se a entidade foi deletada
        public DateTime CreatedAt { get; set; }  // Data de criação da entidade
        public DateTime? DeactivatedAt { get; private set; }  // Data de desativação da entidade
        public DateTime ExpiresAt { get; set; }  // A data e hora de expiração
    }
}
