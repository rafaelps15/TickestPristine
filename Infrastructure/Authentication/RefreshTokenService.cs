using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;

namespace Tickest.Infrastructure.Authentication;

public class RefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenService(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork ?? throw new TickestException(nameof(unitOfWork));

    public async Task<RefreshToken> GenerateRefreshToken(Guid userId, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString(), // Gerar um novo token
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7), // Definir expiração em 7 dias
            IsRevoked = false,
            IsUsed = false
        };

        // Usando o GenericRepository para salvar
        await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken); // Confirma as alterações no banco de dados

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository
                                             .FindAsync(r => r.Token == token && !r.IsRevoked, cancellationToken);
        return refreshToken != null && refreshToken.Any();
    }

    public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository
                                             .FindAsync(r => r.Token == token, cancellationToken);
        var tokenToRevoke = refreshToken?.FirstOrDefault();
        if (tokenToRevoke == null)
            throw new TickestException("Refresh token não encontrado.", nameof(token));

        tokenToRevoke.IsRevoked = true;
        await _unitOfWork.RefreshTokenRepository.UpdateAsync(tokenToRevoke, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task MarkAsUsedAsync(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository
                                             .FindAsync(r => r.Token == token, cancellationToken);
        var tokenToMark = refreshToken?.FirstOrDefault();
        if (tokenToMark == null)
            throw new TickestException("Refresh token não encontrado.", nameof(token));

        tokenToMark.IsUsed = true;
        await _unitOfWork.RefreshTokenRepository.UpdateAsync(tokenToMark, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken); 
    }
}
