using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    ILogger<LoginUserCommandHandler> logger)
    : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null || !passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new TickestException("E-mail ou senha inválidos.");
        }

        if (!user.IsActive || user.IsDeleted)
        {
            throw new TickestException("Usuário inativo ou removido.");
        }

        var tokenResponse = tokenProvider.Create(user);

        if (string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
        {
            throw new TickestException("Falha ao gerar o token.");
        }

        var response = new LoginUserResponse(
            tokenResponse.AccessToken,
            tokenResponse.TokenType,
            tokenResponse.ExpiresAt);

        logger.LogInformation("Usuário {Email} autenticado com sucesso.", command.Email);

        return Result.Success(response);
    }
}
