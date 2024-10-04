using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;

namespace Tickest.Application.Users.CriarUsuario;

public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, CriarUsuarioResponse>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly IHasherDeSenha _hasherDeSenha;
    private readonly ILogger<CriarUsuarioCommandHandler> _logger;

    public CriarUsuarioCommandHandler(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration,
        IHasherDeSenha hasherDeSenha,
        ILogger<CriarUsuarioCommandHandler> logger)
        => (_usuarioRepository, _configuration, _hasherDeSenha,  _logger) = (usuarioRepository, configuration, hasherDeSenha, logger);

    public async Task<CriarUsuarioResponse> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando a criação de usuário: {Email}", request.Email);


        if (await _usuarioRepository.ExisteEmailCadastroAsync(request.Email))
        {
            _logger.LogWarning("Tentativa de cadastro com email já existente: {Email}", request.Email);
            throw new TickestException("Email já cadastrado.");
        }

        var (senhaCriptografada, senhaSalt) = CriarHashSenha(request.Senha);

        var usuario = new Usuario
        {
            Nome = request.Nome,
            Email = request.Email,
            Senha = senhaCriptografada,
            Salt = senhaSalt,
            DataCadastro = DateTime.UtcNow
        };

        await _usuarioRepository.AddAsync(usuario, cancellationToken);

        _logger.LogInformation("Usuário criado com sucesso: {Email}", request.Email);
        return new CriarUsuarioResponse();
    }

    private (string senhaCriptografada, string senhaSalt) CriarHashSenha(string senha)
    {
        var senhaSalt = _hasherDeSenha.GerarSalt();
        var senhaCriptografada = _hasherDeSenha.HashSenha(senha, senhaSalt);
        return (senhaCriptografada, senhaSalt);
    }
}
