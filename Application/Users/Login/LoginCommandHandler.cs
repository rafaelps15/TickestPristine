using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Domain.Interfaces;

namespace Tickest.Application.Users.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<string>>
{
    #region Dependências
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginCommandHandler> _logger;
    #endregion

    #region Construtor
    public LoginCommandHandler(
        IAuthService authService,
        IUnitOfWork unitOfWork,
        ILogger<LoginCommandHandler> logger)
        => (_authService, _unitOfWork, _logger) =
           (authService, unitOfWork, logger);
    #endregion

    #region Método Handle
    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        // Validar se o usuário existe e se as credenciais estão corretas
        var user = await _authService.ValidateUserCredentialsAsync(command.Email, command.Password, cancellationToken);

        // Obter o usuário atual autenticado
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        // Se o usuário atual não estiver autenticado, lançar uma exceção
        if (currentUser == null)
        {
            throw new TickestException("Usuário não autenticado.");
        }

        // Recalcular a senha se necessário (verificar se precisa rehash)
        if (_authService.RehashIfNeeded(command.Password, user.PasswordHash) != null)
        {
            await _authService.RehashPasswordAsync(user, command.Password, cancellationToken);
        }

        // Gerar o token de autenticação para o usuário
        var token = _authService._tokenProvider.GenerateToken(user);

        // Logar a autenticação bem-sucedida
        _logger.LogInformation($"Usuário {command.Email} autenticado com sucesso.");

        // Retornar o token gerado
        return Result.Success(token.AccessToken);
    }
    #endregion
}
