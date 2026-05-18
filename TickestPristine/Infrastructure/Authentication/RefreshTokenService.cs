using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Infrastructure.Authentication
{
    public class RefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;

        public RefreshTokenService(IUnitOfWork unitOfWork, IBaseRepository<RefreshToken> refreshTokenRepository) =>
            (_unitOfWork, _refreshTokenRepository) = (unitOfWork ?? throw new TickestException(nameof(unitOfWork)), refreshTokenRepository);

        public async Task<RefreshToken> GenerateRefreshToken(Guid userId, CancellationToken cancellationToken)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7), // Expira em 7 dias
                IsRevoked = false,
                IsUsed = false
            };

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return refreshToken;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            // Tenta encontrar o token no repositório
            var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token && !r.IsRevoked, cancellationToken);

            if (refreshToken == null)
                return false;

            return refreshToken.IsValid();
        }


        public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            // Tenta encontrar o token no repositório
            var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);

            if (refreshToken == null)
                throw new TickestException("Refresh token não encontrado.", nameof(token));

            // Marca o token como revogado
            refreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }


        public async Task MarkAsUsedAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);

            if (refreshToken == null)
                throw new TickestException("Refresh token não encontrado.", nameof(token));

            // Marca o token como usado
            refreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

    }
}
