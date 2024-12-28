// Camada Domain - Tickest.Domain.Interfaces.Repositories
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Domain.Interfaces.Repositories;

public interface ISpecialtyRepository : IBaseRepository<Specialty>
{
    Task<ICollection<Specialty>> GetSpecialtiesByNamesAsync(IEnumerable<string> specialtyNames, CancellationToken cancellationToken);
}
