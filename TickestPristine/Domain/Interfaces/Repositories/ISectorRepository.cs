﻿using Tickest.Domain.Entities.Departments;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ISectorRepository : IBaseRepository<Sector>
{
    Task<Sector> GetByIdWithDetailsAsync(Guid sectorId);
}
