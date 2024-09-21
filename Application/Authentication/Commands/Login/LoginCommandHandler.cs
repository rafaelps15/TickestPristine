using MediatR;
using Microsoft.Extensions.Options;
using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Helpers;
using Tickest.Infrastructure.Services.Auth;

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

    #region Utilities

    private bool MatchPassword(Usuario usuario, string enteredPassword)
    {
        var savedPassword = PasswordHasher.HashPassword(enteredPassword, usuario.Salt);
        return usuario.Senha.Equals(savedPassword);
    }

    #endregion

    public async Task<TokenModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

        if (usuario == null)
            throw new TickestException("Usuário não encontrado.");

        if (!MatchPassword(usuario, request.Senha))
            throw new TickestException("Senha incorreta.");

        var authResult = await _authService.AuthenticateAsync(usuario);

        return authResult;
    }
    public async Task<Usuario>ValidateUserAsync(string email, string senha)
    {
        var usuario = await _usuarioRepository.UpdateAsync

    }
}
