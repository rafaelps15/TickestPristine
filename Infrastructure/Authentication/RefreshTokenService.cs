using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Infrastructure.Authentication;

public class RefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository; 

    public RefreshTokenService(IUnitOfWork unitOfWork, IGenericRepository<RefreshToken> refreshTokenRepository) =>
        (_unitOfWork, _refreshTokenRepository) = (unitOfWork ?? throw new TickestException(nameof(unitOfWork)), refreshTokenRepository);

    public async Task<RefreshToken> GenerateRefreshToken(Guid userId, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString(), // Gerar um novo token
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7), // Expiração em 7 dias
            IsRevoked = false,
            IsUsed = false
        };

        // Usando o GenericRepository para salvar
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken); // Confirma a transação

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token && !r.IsRevoked, cancellationToken);
        return refreshToken is not null && refreshToken.Any();
    }

    public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        // Buscando e revogando o refresh token
        var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);
        var tokenToRevoke = refreshToken?.FirstOrDefault();
        if (tokenToRevoke is null)
            throw new TickestException("Refresh token não encontrado.", nameof(token));

        tokenToRevoke.IsRevoked = true;
        await _refreshTokenRepository.UpdateAsync(tokenToRevoke, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken); 
    }

    public async Task MarkAsUsedAsync(string token, CancellationToken cancellationToken)
    {
        // Buscando o refresh token para marcar como usado
        var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);
        var tokenToMark = refreshToken?.FirstOrDefault();
        if (tokenToMark is null)
            throw new TickestException("Refresh token não encontrado.", nameof(token));

        tokenToMark.IsUsed = true;
        await _refreshTokenRepository.UpdateAsync(tokenToMark, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken); 
    }
}
