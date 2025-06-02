using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class SpecialtyRepository : BaseRepository<Specialty,Guid>, ISpecialtyRepository
{
    private readonly TickestContext _context;

    public SpecialtyRepository(TickestContext context) : base(context) =>
        _context = context;

    public async Task<ICollection<Specialty>> GetSpecialtiesByNamesAsync(IEnumerable<string> specialtyNames, CancellationToken cancellationToken)
    {
        // Realiza busca das as especialidades pelos nomes
        var specialties = await _context.Specialties
            .Where(s => specialtyNames.Contains(s.Name))  // Filtra pelo nome da especialidade
            .ToListAsync(cancellationToken);  

        return specialties;
    }

   
}
