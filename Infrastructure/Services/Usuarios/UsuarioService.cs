using Tickest.Domain.Entities;
using Tickest.Persistence.Repositories;

namespace Tickest.Infrastructure.Services.Usuarios;

public class UsuarioService : IUsuarioService
{
	private readonly IUsuarioRepository _usuarioRepository;

	public UsuarioService(IUsuarioRepository usuarioRepository)
	{
		_usuarioRepository = usuarioRepository;
	}

	public async Task<bool> ExisteEmailCadastroAsync(string email)
	{
		return await _usuarioRepository.ExisteEmailCadastroAsync(email);
	}

	public async Task<Usuario> ObterUsuarioPorEmailAsync(string email)
	{
		return await _usuarioRepository.ObterUsuarioPorEmailAsync(email);
	}

	public async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
	{
		return await _usuarioRepository.ValidarUsuarioAsync(email, senha);
	}

}
