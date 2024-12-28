﻿using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    protected readonly TickestContext _context;

    public DepartmentRepository(TickestContext context) : base(context) => _context = context;
}
