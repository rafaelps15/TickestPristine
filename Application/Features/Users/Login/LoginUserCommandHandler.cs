using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Login;

internal sealed class LoginUserCommandHandler(
    IAuthService authService,
    IUserRepository userRepository)
    : ICommandHandler<LoginUserCommand,string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmailAsync(command.Email, cancellationToken); 

        if (user == null)
        {
            throw new TickestException("Usuário não encontrado.");
        }

        var authenticationToken = await authService.AuthenticateAsync(command.Email, command.Password, cancellationToken);

        if (authenticationToken == null || string.IsNullOrEmpty(authenticationToken.AccessToken))
        {
            throw new TickestException("Falha ao gerar o token.");
        }

        return authenticationToken.AccessToken;
    }
}
