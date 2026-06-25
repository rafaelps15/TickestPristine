using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(TickestContext context) : base(context) { }
}
