using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Repositories;

internal class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TickestContext context) : base(context)
    {
    }

    public async Task<bool> ExisteEmailCadastroAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(e => e.Email == email.ToLower());
    }

    public async Task<Usuario> ObterUsuarioPorEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
    {
        return await _context.Usuarios.SingleOrDefaultAsync(p => p.Email == email && p.Senha == senha);
    }
}
