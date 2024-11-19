namespace Tickest.Domain.Entities;

public class UserSpecialty : EntityBase
{
    #region Properties

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; }

    // Para saber se a especialidade é principal ou não
    public bool IsMainSpecialty { get; set; }

    #endregion
}
