using MediatR;
using Microsoft.Extensions.Options;
using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Helpers;
using Tickest.Infrastructure.Services.Auth;
using Tickest.Persistence.Repositories;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenModel>
{
	private readonly IAuthService _authService;
	private readonly IUsuarioRepository _usuarioRepository;
	private readonly JwtConfiguracao _jwtConfiguracao;

	public LoginCommandHandler(
		IAuthService authService,
		IUsuarioRepository usuarioRepository,
		IOptions<JwtConfiguracao> jwtConfiguracao)
	{
		_authService = authService;
		_usuarioRepository = usuarioRepository;
		_jwtConfiguracao = jwtConfiguracao.Value;
	}

	public async Task<TokenModel> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
		{
			throw new TickestException("Email e senha devem ser informados.");
		}

		var usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(request.Email)
					  ?? throw new TickestException("Usuário não encontrado.");

		var hashedPassword = HasherDeSenha.HashSenha(request.Senha, usuario.Salt);

		if (!hashedPassword.Equals(usuario.Senha))
		{
			throw new TickestException("Senha incorreta.");
		}

		return await _authService.AuthenticateAsync(usuario);
	}

}
