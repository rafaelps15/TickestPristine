using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IAuthService authService,
    ILogger<LoginUserCommandHandler> logger)
    : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var tokenResponse = await authService.AuthenticateAsync(command.Email, command.Password, cancellationToken);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
        {
            throw new TickestException("Falha ao gerar o token.");
        }

        logger.LogInformation("Usuário {Email} autenticado com sucesso.", command.Email);
        return Result.Success(tokenResponse.Token);
    }
}
