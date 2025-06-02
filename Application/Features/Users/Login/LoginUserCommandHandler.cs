using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Features.Users.Login;

internal sealed class LoginUserCommandHandler(
    IAuthService authService,
    ILogger<LoginUserCommandHandler> logger) : ICommandHandler<LoginUserCommand,string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var authenticationToken = await authService.AuthenticateAsync(command.Email, command.Password, cancellationToken);

        if (authenticationToken == null || string.IsNullOrEmpty(authenticationToken.AccessToken))
        {
            throw new TickestException("Falha ao gerar o token.");
        }

        return Result.Success(authenticationToken.AccessToken);
    }
}
