using MediatR;
using Microsoft.Extensions.Options;
using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Helpers;
using Tickest.Infrastructure.Interfaces;
using Tickest.Persistence.Repositories;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenModel>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtConfiguracao _jwtConfiguracao;

    public LoginCommandHandler(
        IAuthenticationService authenticationService,
        IUsuarioRepository usuarioRepository,
        IOptions<JwtConfiguracao> jwtConfiguracao)
        => (_authenticationService, _usuarioRepository, _jwtConfiguracao) = (authenticationService, usuarioRepository, jwtConfiguracao.Value);

    public async Task<TokenModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        ValidarCredenciais(request);

        var usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(request.Email)
                      ?? throw new TickestException("Usuário não encontrado.");

        VerificarSenha(request.Senha, usuario.Salt, usuario.Senha);

        return await _authenticationService.AuthenticateAsync(usuario);
    }

    private void ValidarCredenciais(LoginCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            throw new TickestException("Email e senha devem ser informados.");
    }

    private void VerificarSenha(string senha, string salt, string senhaArmazenada)
    {
        var hasher = new HasherDeSenha();
        var hashedPassword = hasher.HashSenha(senha, salt);

        if (!hashedPassword.Equals(senhaArmazenada))
            throw new TickestException("Senha incorreta.");
    }
}
