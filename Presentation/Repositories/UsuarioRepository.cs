using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class UsuarioRepository : BaseRepository<User>, IUserRepository
{
    public UsuarioRepository(TickestContext context) : base(context)
    {
    }

    public async Task<User> ObterUsuarioPorIdAsync(int usuarioId)
    {
        return await GetByIdAsync(usuarioId); 
    }

    public async Task<bool> DoesEmailExistAsync(string usuarioEmail)
    {
        return await _context.Users.AnyAsync(e => e.Email == usuarioEmail.ToLower());
    }

    public async Task<User> ObterUsuarioPorEmailAsync(string usuarioEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == usuarioEmail);
    }

    public async Task<ICollection<UserRole>> ObterRegrasUsuarioAsync(int usuarioId)
    {
        return await _context.UserRoles.Where(p => p.UserId == usuarioId).ToListAsync();
    }
}
