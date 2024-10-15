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

    public async Task<Usuario> ObterUsuarioPorIdAsync(int usuarioId)
    {
        return await GetByIdAsync(usuarioId); 
    }

    public async Task<bool> ExisteEmailCadastroAsync(string usuarioEmail)
    {
        return await _context.Usuarios.AnyAsync(e => e.Email == usuarioEmail.ToLower());
    }

    public async Task<Usuario> ObterUsuarioPorEmailAsync(string usuarioEmail)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuarioEmail);
    }

    public async Task<ICollection<UsuarioRegra>> ObterRegrasUsuarioAsync(int usuarioId)
    {
        return await _context.UsuarioRegras.Where(p => p.UsuarioId == usuarioId).ToListAsync();
    }
}
