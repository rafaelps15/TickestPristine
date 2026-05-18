using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(TickestContext context) : base(context) { }

    #region Métodos de Consulta

    public async Task<User?> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .FirstOrDefaultAsync(u => u.Email == userEmail);

    public async Task<User?> GetByNameAsync(string userName) =>
        await _context.Set<User>()
                      .AsNoTracking()
                      .FirstOrDefaultAsync(user => user.Name == userName);

    public async Task<User?> GetByIdWithDetailsAsync(Guid userId, CancellationToken cancellationToken) =>
        await _context.Users
                      .AsNoTracking()
                      .Include(user => user.UserSpecialties)
                          .ThenInclude(userSpecialty => userSpecialty.Specialty)
                      .Include(user => user.Permissions)
                      .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

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
