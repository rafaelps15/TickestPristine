using Tickest.Domain.Entities;

namespace Tickest.Domain.Repositories;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<bool> ExisteEmailCadastroAsync(string email);
    Task<Usuario> ObterUsuarioPorEmailAsync(string email);
    Task<Usuario> ObterUsuarioPorIdAsync(int userId);
    Task<Usuario> ValidarUsuarioAsync(string email, string senha);
}

