using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;

namespace Tickest.Application.Users.AtualizarUsuario;

public class AtualizarUsuarioCommandHandler : IRequestHandler<AtualizarUsuarioCommand, AtualizarUsuarioResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly IHasherDeSenha _hasherDeSenha;
    private readonly ILogger<AtualizarUsuarioCommandHandler> _logger;

    public AtualizarUsuarioCommandHandler(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration,
        IHasherDeSenha hasherDeSenha,
        ILogger<AtualizarUsuarioCommandHandler> logger)
        => (_usuarioRepository, _configuration, _hasherDeSenha, _logger) = (usuarioRepository, configuration, hasherDeSenha, logger);

    public async Task<AtualizarUsuarioResponse> Handle(AtualizarUsuarioCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        // Buscar o usuário no repositório
        var usuario = await _usuarioRepository.ObterUsuarioPorIdAsync(request.UsuarioId)
         ?? throw new TickestException($"Usuário com ID {request.UsuarioId} não encontrado.");

        AtualizarPropriedadesUsuario(usuario, request);
        await _usuarioRepository.UpdateAsync(usuario);

        _logger.LogInformation($"Usuário com ID {usuario.Id} atualizado com sucesso.");
        return MapearResposta(usuario);

    }

    private void AtualizarPropriedadesUsuario(Usuario usuario, AtualizarUsuarioCommand request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "O comando de atualização não pode ser nulo.");
        }

        usuario.Email = request.Email;
        usuario.Nome = request.Nome;

        if (!string.IsNullOrWhiteSpace(request.Senha))
        {
            var salt = _hasherDeSenha.GerarSalt();// Gerar um novo salt para a nova senha
            usuario.Senha = _hasherDeSenha.HashSenha(request.Senha, salt);// Hash da nova senha
        }
    }

    private AtualizarUsuarioResponse MapearResposta(Usuario usuario) =>
        new AtualizarUsuarioResponse(usuario.Id, usuario.Email, usuario.Nome);
}
