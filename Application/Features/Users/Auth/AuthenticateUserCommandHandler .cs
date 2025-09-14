using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.DTOs;
using Tickest.Domain.Common;
using Tickest.Domain.Common.Auth;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Auth;

internal sealed class AuthenticateUserCommandHandler(
    ITokenProvider tokenProvider,
    JwtSettings jwtSettings,
    IUserRepository userRepository,
    ILogger<AuthenticateUserCommand> logger)
    : ICommandHandler<AuthenticateUserCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmailAsync(command.Email, cancellationToken)
                   ?? throw new TickestException("Usuário não encontrado.");

        if (!user.IsActive)
            throw new TickestException("Usuário inativo. Contate o administrador.");
        if (user.IsDeleted)
            throw new TickestException("Usuário deletado. Contate o administrador.");
        if (!EncryptionHelper.CheckPassword(command.Password.ToLower(), user.Salt, user.PasswordHash))
            throw new TickestException("Credenciais inválidas.");

        var accessToken = tokenProvider.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes);

        logger.LogInformation("Usuário {UserId} autenticado com sucesso.", user.Id);

        return Result.Success(new TokenResponse
        {
            UserId = user.Id,
            Email = user.Email,
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            TokenType = "Bearer"
        }); 
    }
}
