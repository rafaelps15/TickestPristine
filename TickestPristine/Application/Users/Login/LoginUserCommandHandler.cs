﻿using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IPasswordHasher passwordHasher,
    IAuthService authService,
    ITokenProvider tokenProvider,
    IPermissionProvider permissionProvider,
    ILogger<LoginUserCommandHandler> logger)
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        // Reaproveita o método AuthenticateAsync do AuthService
        var tokenResponse = await authService.AuthenticateAsync(command.Email, command.Password, cancellationToken);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
        {
            throw new TickestException("Falha ao gerar o token.");
        }

        logger.LogInformation($"Usuário {command.Email} autenticado com sucesso.");

        // Após a autenticação, verifica as permissões do usuário
        var user = await authService.GetCurrentUserAsync(cancellationToken);

        await permissionProvider.CanUserLoginAsync(user.Id);

        logger.LogInformation($"Usuário {command.Email} autenticado com sucesso.");

        // Retorna o token gerado
        return Result.Success(tokenResponse.Token);
    }
}
