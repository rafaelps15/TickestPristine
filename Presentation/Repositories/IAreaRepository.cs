using Tickest.Domain.Entities;


namespace Tickest.Persistence.Repositories;

public interface IAreaRepository : IBaseRepotirory<Area>
{
	Task<ICollection<Area>> GetByDescription(string description);
}
