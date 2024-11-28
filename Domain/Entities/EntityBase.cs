namespace Tickest.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; } 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } 
        public DateTime? DeactivatedAt { get; set; } 
    }
}
