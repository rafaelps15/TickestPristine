using MediatR;
using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Repositories;
using Tickest.Infrastructure.Helpers;
using Tickest.Infrastructure.Services.Auth;

namespace Tickest.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenModel>
{
    private readonly IAuthService _authService;
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginCommandHandler(
        IAuthService authService,
        IUsuarioRepository usuarioRepository)
    {
        this._authService = authService;
        _usuarioRepository = usuarioRepository;
    }

    #region Utilities

    private bool MatchPassord(Usuario usuario, string enteredPassword)
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

        if(!MatchPassord(usuario, request.Senha))
            throw new TickestException("Senha incorreta.");

        var authResult = await _authService.AuthenticateAsync(usuario);

        throw new NotImplementedException();
    }
} 
