using Microsoft.AspNetCore.Identity;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Helpers;
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
		var usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(email)
			?? throw new TickestException("Usuário não encontrado.");

		return usuario;
	}

	public async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
	{
		var usuario = await ObterUsuarioPorEmailAsync(email)
					   ?? throw new TickestException("Usuário não encontrado.");

		var hashedPassword = HasherDeSenha.HashSenha(senha, usuario.Salt);

		if (!hashedPassword.Equals(usuario.Senha))
			throw new TickestException("Senha incorreta.");

		return usuario;
	}


}
