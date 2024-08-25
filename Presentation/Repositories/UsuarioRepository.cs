using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;

namespace Tickest.Persistence.Repositories;

internal class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TickestContext context) : base(context)
    {
    }

    public async Task<bool> ExisteUsuarioEmailAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(p => p.Email == email.ToLower());
    }
}
