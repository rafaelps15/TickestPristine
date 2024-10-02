using Tickest.Domain.Entities;


namespace Tickest.Domain.Repositories;

public interface IAreaRepository : IBaseRepository<Area>
{
    Task<ICollection<Area>> GetByDescription(string description);
}
