using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IAuthService authService,
    ITokenProvider tokenProvider,
    IPermissionProvider permissionProvider,
    ILogger<LoginUserCommandHandler> logger) : ICommandHandler<LoginUserCommand,string>
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

        //Verificar se será necessário verificar o papel do usuario para fazer login 

        logger.LogInformation($"Usuário {command.Email} autenticado com sucesso.");

        // Retorna o token gerado
        return Result.Success(tokenResponse.Token);
    }
}
