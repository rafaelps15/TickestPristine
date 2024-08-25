using Tickest.Domain.Entities;

namespace Tickest.Domain.Repositories;

public interface IAreaRepository : IBaseRepotirory<Area>
{
    Task<ICollection<Area>> GetByDescription(string description);
}
