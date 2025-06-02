using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(TickestContext context) : base(context) { }

    #region Métodos de Consulta

    public async Task<User?> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .Include(p => p.UserRoles)
                          .ThenInclude(ur => ur.Role)
                      .FirstOrDefaultAsync(u => u.Email == userEmail, cancellationToken);

    public async Task<User?> GetByNameAsync(string userName) =>
         await _context.Users
                      .AsNoTracking()
                      .FirstOrDefaultAsync(user => user.Name == userName);

    public async Task<List<Role>> GetUserRolesAsync(User currentUser, CancellationToken cancellationToken) =>
        await _context.UserRoles
                      .Where(ur => ur.UserId == currentUser.Id)
                      .Include(ur => ur.Role)
                      .Select(ur => ur.Role)
                      .ToListAsync(cancellationToken);

    #endregion

    #region Métodos de Verificação

    public async Task<bool> DoesEmailExistAsync(string userEmail, CancellationToken cancellationToken) =>
        await _context.Users
                     .AsNoTracking()
                     .AnyAsync(u => u.Email == userEmail, cancellationToken);

    public async Task<bool> AnyUsersExistAsync(CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .AnyAsync(cancellationToken);

    #endregion
}
