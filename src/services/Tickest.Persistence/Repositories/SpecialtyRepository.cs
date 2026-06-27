using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public sealed class SpecialtyRepository(ApplicationDbContext context)
    : Repository<Specialty>(context), ISpecialtyRepository
{
    public async Task<ICollection<Specialty>> GetSpecialtiesByNamesAsync(
        IEnumerable<string> specialtyNames,
        CancellationToken cancellationToken)
    {
        var names = specialtyNames.ToArray();

        return await DbSet
            .Where(s => names.Contains(s.Name))
            .ToListAsync(cancellationToken);
    }
}
