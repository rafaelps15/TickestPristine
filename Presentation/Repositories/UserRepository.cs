using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class UserRepository : BaseRepository<User>, IUserRepository
{
    // Construtor que recebe o contexto
    public UserRepository(TickestContext context) : base(context) { }

    // Método para buscar o usuário pelo e-mail
    public async Task<User> GetUserByEmailAsync(string userEmail) =>
        await _context.Users
                      .AsNoTracking()
                      .FirstOrDefaultAsync(u => u.Email == userEmail);

    // Método para verificar se o e-mail já existe no banco de dados
    public async Task<bool> DoesEmailExistAsync(string userEmail) =>
        await _context.Users
                      .AsNoTracking()
                      .AnyAsync(u => u.Email == userEmail);

    // Método para buscar os papéis do usuário com suas permissões
    //public async Task<List<UserRole>> GetUserRolesAsync(Guid userId) =>
    //    await _context.UserRoles
    //                   .Where(ur => ur.UserId == userId)
    //                   .Include(ur => ur.Role)
    //                   .ThenInclude(r => r.Permissions) // Inclui as permissões associadas ao papel
    //                   .AsNoTracking()
    //                   .ToListAsync();

    //// Método para obter apenas os nomes dos papéis do usuário
    //public async Task<IEnumerable<string>> GetUserRoleNamesAsync(Guid userId) =>
    //    await _context.UserRoles
    //                  .Where(ur => ur.UserId == userId)
    //                  .Select(ur => ur.Role.Name)
    //                  .AsNoTracking()
    //                  .ToListAsync();

    //// Método para verificar se o usuário possui uma permissão específica
    //public async Task<bool> UserHasPermissionAsync(Guid userId, string permission) =>
    //    await _context.UserRoles
    //                  .Where(ur => ur.UserId == userId)
    //                  .AnyAsync(ur => ur.Role.Permissions.Any(p => p.Name == permission));
}
