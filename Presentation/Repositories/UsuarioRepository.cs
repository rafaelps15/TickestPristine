using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TickestContext context) : base(context)
    {
    }

    public async Task<Usuario> ObterUsuarioPorIdAsync(int id)
    {
        return await GetByIdAsync(id); 
    }

    public async Task<bool> ExisteEmailCadastroAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(e => e.Email == email.ToLower());
    }

    public async Task<Usuario> ObterUsuarioPorEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ICollection<UsuarioRegra>> ObterRegrasUsuarioAsync(int usuarioId)
    {
        return await _context.UsuarioRegras.Where(p => p.UsuarioId == usuarioId).ToListAsync();
    }
}
