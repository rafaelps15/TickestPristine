using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<string>>
{
    #region Dependencies
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtConfiguration _jwtConfiguration;
    #endregion

    #region Constructor
    public LoginCommandHandler(
        IAuthService authService,
        IUserRepository userRepository,
        ITokenProvider tokenProvider,
        IPasswordHasher passwordHasher,
        JwtConfiguration jwtConfiguration)
        => (_authService, _userRepository, _tokenProvider, _passwordHasher, _jwtConfiguration) =
           (authService, userRepository, tokenProvider, passwordHasher, jwtConfiguration);
    #endregion

    #region Handle Method
    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        // Validar se o usuário existe pelo email
        var user = await _userRepository.GetUserByEmailAsync(command.Email);

        if (user is null)
        {
            throw new TickestException($"Usuário não encontrado para o email: {command.Email}");
        }

        // Obter o usuário atual
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        if (currentUser == null)
        {
            throw new TickestException("Usuário não autenticado.");
        }

        // Validar a senha
        if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new TickestException("Senha incorreta.");
        }

        // Gerar o token de autenticação
        string token = _tokenProvider.Create(user, _jwtConfiguration.ExpirationInMinutes);

        return Result.Success(token);
    }
    #endregion
}
