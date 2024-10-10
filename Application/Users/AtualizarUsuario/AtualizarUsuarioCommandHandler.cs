using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;

namespace Tickest.Application.Users.AtualizarUsuario
{
	public class AtualizarUsuarioCommandHandler : IRequestHandler<AtualizarUsuarioCommand, AtualizarUsuarioResponse>
	{
		private readonly IUsuarioRepository _usuarioRepository; // Repositório de usuários
		private readonly IConfiguration _configuration; // Configurações da aplicação
		private readonly IHasherDeSenha _hasherDeSenha; // Serviço para hash de senhas
		private readonly ILogger<AtualizarUsuarioCommandHandler> _logger; // Logger para registrar eventos

		// Construtor com injeção de dependências
		public AtualizarUsuarioCommandHandler(
			IUsuarioRepository usuarioRepository,
			IConfiguration configuration,
			IHasherDeSenha hasherDeSenha,
			ILogger<AtualizarUsuarioCommandHandler> logger)
			=> (_usuarioRepository, _configuration, _hasherDeSenha, _logger) = (usuarioRepository, configuration, hasherDeSenha, logger);

		public async Task<AtualizarUsuarioResponse> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
		{
			// Validar o comando usando o validador
			request.Validate();

			// Buscar o usuário no repositório
			var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
			if (usuario == null)
			{
				_logger.LogError($"Usuário com ID {request.Id} não encontrado.");
				throw new TickestException("Usuário não encontrado.");
			}

			// Atualizar as propriedades do usuário
			usuario.Email = request.Email;
			usuario.Nome = request.Nome;

			// Atualizar a senha se foi informada
			if (!string.IsNullOrWhiteSpace(request.Senha))
			{
				usuario.Senha = _hasherDeSenha.HashPassword(request.Senha); // Hash da nova senha
			}

			// Persistir as alterações no repositório
			await _usuarioRepository.UpdateAsync(usuario);

			_logger.LogInformation($"Usuário com ID {usuario.Id} atualizado com sucesso.");

			return new AtualizarUsuarioResponse
			{
				Id = usuario.Id,
				Email = usuario.Email,
				Nome = usuario.Nome
			};
		}
	}
}
