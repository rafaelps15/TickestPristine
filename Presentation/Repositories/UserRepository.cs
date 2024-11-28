using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(TickestContext context) : base(context) { }

    #region Métodos de Consulta

    public async Task<User> GetUserByEmailAsync(string userEmail) =>
        await _context.Users
                      .AsNoTracking()
                      .FirstOrDefaultAsync(u => u.Email == userEmail);

    public async Task<User?> GetByNameAsync(string userName) =>
        await _context.Set<User>()
                      .AsNoTracking()
                      .FirstOrDefaultAsync(user => user.Name == userName);

    public async Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId)
    {
        var userRoles = await _context.UserRoles
                                      .AsNoTracking()
                                      .Include(ur => ur.Role)
                                      .Where(ur => ur.UserId == userId)
                                      .ToListAsync();

        return userRoles ?? new List<UserRole>();
    }

    #endregion

    #region Métodos de Verificação

    public async Task<bool> DoesEmailExistAsync(string userEmail, CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .AnyAsync(u => u.Email == userEmail);

    public async Task<bool> AnyUsersExistAsync(CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .AnyAsync(cancellationToken);



    #endregion
}
