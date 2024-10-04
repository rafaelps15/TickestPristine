using MediatR;
using Microsoft.Extensions.Options;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Helpers;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly JwtConfiguracao _jwtConfiguracao;

    public LoginCommandHandler(
        IAuthenticationService authenticationService,
        IUsuarioRepository usuarioRepository,
        IOptions<JwtConfiguracao> jwtConfiguracao)
        => (_authenticationService, _usuarioRepository, _jwtConfiguracao) = (authenticationService, usuarioRepository, jwtConfiguracao.Value);

    #region Utilities 

    private void VerificarSenha(string senha, string salt, string senhaArmazenada)
    {
        var hasher = new HasherDeSenha();
        var hashedPassword = hasher.HashSenha(senha, salt);

        if (!hashedPassword.Equals(senhaArmazenada))
            throw new TickestException("Senha incorreta.");
    }

    #endregion

    public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(request.Email)
                      ?? throw new TickestException("Usuário não encontrado.");

        VerificarSenha(request.Senha, usuario.Salt, usuario.Senha);

        return await _authenticationService.AuthenticateAsync(usuario);
    }
}
