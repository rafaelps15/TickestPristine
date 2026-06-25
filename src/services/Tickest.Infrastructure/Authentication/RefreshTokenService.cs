using Tickest.Domain.Entities.Auths;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;

namespace Tickest.Infrastructure.Authentication;

public class RefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshTokenService(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, IDateTimeProvider dateTimeProvider) =>
        (_unitOfWork, _refreshTokenRepository, _dateTimeProvider) = (unitOfWork ?? throw new TickestException(nameof(unitOfWork)), refreshTokenRepository, dateTimeProvider);

    public async Task<RefreshToken> GenerateRefreshToken(Guid userId, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = _dateTimeProvider.UtcNow.AddDays(7),
            IsRevoked = false,
            IsUsed = false
        };

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token && !r.IsRevoked, cancellationToken);
        return refreshToken?.IsValid(_dateTimeProvider.UtcNow) == true;
    }

    public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);

        if (refreshToken == null)
        {
            throw new TickestException("Refresh token não encontrado.", nameof(token));
        }

        refreshToken.Revoke(_dateTimeProvider.UtcNow);
        await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task MarkAsUsedAsync(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);

        if (refreshToken == null)
        {
            throw new TickestException("Refresh token não encontrado.", nameof(token));
        }

        refreshToken.MarkAsUsed(_dateTimeProvider.UtcNow);
        await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
