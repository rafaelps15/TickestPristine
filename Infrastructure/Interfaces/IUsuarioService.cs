using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Interfaces;

public interface IUsuarioService
{
    Task<bool> ExisteEmailCadastroAsync(string email);
    Task<Usuario> ObterUsuarioPorEmailAsync(string email);
    Task<Usuario> ValidarUsuarioAsync(string email, string senha);
}
