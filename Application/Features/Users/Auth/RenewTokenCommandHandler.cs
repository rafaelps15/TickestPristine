using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Auth;

internal sealed class RenewTokenCommandHandler(
    ILogger<RenewTokenCommandHandler> logger,
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    ITokenProvider tokenProvider
) : ICommandHandler<RenewTokenCommand, string>
{
    public async Task<Result<string>> Handle(RenewTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshTokenEntity == null || refreshTokenEntity.ExpiresAt < DateTime.UtcNow)
            throw new TickestException("Refresh token inválido ou expirado.");

        var user = await userRepository.GetByIdAsync(refreshTokenEntity.UserId)
                   ?? throw new TickestException("Usuário associado ao refresh token não encontrado.");

        var token = tokenProvider.GenerateToken(user); // retorna string

        logger.LogInformation("Refresh token renovado para o usuário {UserId}", user.Id);

        return Result.Success(token);
    }
}
/// <summary>
/// Fluxo de renovação de token para manter o usuário logado:
/// 
/// 1. Login → Recebe accessToken + refreshToken
/// 2. AccessToken expira → Front envia refreshToken
/// 3. RenewTokenCommandHandler valida token e busca usuário
/// 4. Gera novo accessToken → Retorna ao front
/// 5. Front continua usando novo accessToken
/// </summary>
