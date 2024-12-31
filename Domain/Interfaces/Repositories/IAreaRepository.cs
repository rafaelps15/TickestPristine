﻿using Tickest.Domain.Entities.Departments;


namespace Tickest.Domain.Interfaces.Repositories;

public interface IAreaRepository : IBaseRepository<Area>
{
    Task<List<Area>> GetAreasWithSpecialtiesByIdsAsync(List<Guid> areaIds, CancellationToken cancellationToken);
}
