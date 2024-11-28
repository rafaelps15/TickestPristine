using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly TickestContext _context;

    public RoleRepository(TickestContext context) : base(context) =>
        _context = context;

    // Método para obter a role por nome
    public async Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken) =>
             await _context.Roles
            .AsNoTracking() // Melhora o desempenho em leituras
            .FirstOrDefaultAsync(role => role.Name == name, cancellationToken);


    // Método para obter roles de um usuário
    public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken) =>
             await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .ToListAsync(cancellationToken);


    // Método para obter a primeira role que corresponda aos nomes fornecidos
    public async Task<Role> GetFirstRoleByNamesAsync(string[] roleNames, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .AsNoTracking() // Melhora o desempenho
            .Where(role => roleNames.Contains(role.Name))
            .FirstOrDefaultAsync(cancellationToken); // Retorna a primeira role ou null

        return role;
    }

    // Método para obter a role correspondente aos nomes fornecidos
    public async Task<Role> GetRoleByNamesAsync(string[] roleNames, CancellationToken cancellationToken) =>
             await _context.Roles
            .AsNoTracking() // Melhora o desempenho
            .Where(role => roleNames.Contains(role.Name))
            .FirstOrDefaultAsync(cancellationToken); // Retorna a primeira role ou null


    // Método para obter todas as roles pelos nomes fornecidos
    public async Task<IList<Role>> GetAllRolesByNamesAsync(IReadOnlyList<string> roleNames, CancellationToken cancellationToken) =>
             await _context.Roles
            .AsNoTracking() // Melhora o desempenho
            .Where(role => roleNames.Contains(role.Name))
            .ToListAsync(cancellationToken);

}
