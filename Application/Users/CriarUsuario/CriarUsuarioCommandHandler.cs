using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Helpers;
using Tickest.Persistence.Repositories;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, Unit>
{
	private readonly IUsuarioRepository _usuarioRepository;
	private readonly IConfiguration _configuration;

	public CriarUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IConfiguration configuration)
		=> (_usuarioRepository, _configuration) = (usuarioRepository, configuration);

	private bool IsValidEmail(string email)
	{
		return MailAddress.TryCreate(email, out _);
	}

	private bool SenhaAtendeCritérios(string senha)
	{
		// Senha com pelo menos uma letra maiúscula, dois caracteres especiais e 8 caracteres de comprimento
		var senhaRegex = new Regex(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$");
		int especialCount = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]").Matches(senha).Count;

		return senhaRegex.IsMatch(senha) && especialCount >= 2;
	}

	public async Task<Unit> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
			throw new TickestException("Email inválido.");

		if (!SenhaAtendeCritérios(request.Senha))
			throw new TickestException("A senha deve ter pelo menos 8 caracteres, incluir pelo menos uma letra maiúscula e dois caracteres especiais.");

		if (await _usuarioRepository.ExisteEmailCadastroAsync(request.Email))
			throw new TickestException("Email já cadastrado.");

		// Criar o salt e criptografar a senha
		var senhaSalt = HasherDeSenha.GerarSalt();
		string senhaCriptografada = HasherDeSenha.HashSenha(request.Senha, senhaSalt);

		var usuario = new Usuario
		{
			Nome = request.Nome,
			Email = request.Email,
			Senha = senhaCriptografada,
			Salt = senhaSalt,
			DataCadastro = DateTime.UtcNow
		};

		await _usuarioRepository.AddAsync(usuario,cancellationToken);

		return Unit.Value; // Retornar um valor padrão do MediatR
	}
}
