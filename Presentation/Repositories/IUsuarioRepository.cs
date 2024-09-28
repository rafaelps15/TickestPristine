using Tickest.Domain.Entities;

namespace Tickest.Persistence.Repositories;

public interface IUsuarioRepository : IBaseRepotirory<Usuario>
{
	Task<bool> ExisteEmailCadastroAsync(string email, CancellationToken cancellationToken);
	Task<Usuario> ObterUsuarioPorEmailAsync(string email);
	Task<Usuario> ValidarUsuarioAsync(string email, string senha);
}

