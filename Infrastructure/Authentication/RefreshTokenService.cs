using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Infrastructure.Authentication
{
    public class RefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<RefreshToken,Guid> _refreshTokenRepository;

        public RefreshTokenService(IUnitOfWork unitOfWork, IBaseRepository<RefreshToken,Guid> refreshTokenRepository) =>
            (_unitOfWork, _refreshTokenRepository) = (unitOfWork ?? throw new TickestException(nameof(unitOfWork)), refreshTokenRepository);

        /// <summary>
        /// Gera um novo RefreshToken para um usuário.
        /// </summary>
        /// <param name="userId">ID do usuário para o qual o RefreshToken será gerado.</param>
        /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
        /// <returns>O RefreshToken gerado.</returns>
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
            await _unitOfWork.CommitTransactionAsync();

            return refreshToken;
        }

        /// <summary>
        /// Valida se o RefreshToken é válido.
        /// </summary>
        /// <param name="token">Token a ser validado.</param>
        /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
        /// <returns>True se o token for válido, caso contrário, false.</returns>
        public async Task<bool> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            // Tenta encontrar o token no repositório
            var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token && !r.IsRevoked, cancellationToken);

            if (refreshToken == null)
                return false;

            return refreshToken.IsValid();
        }


        /// <summary>
        /// Revoga um RefreshToken.
        /// </summary>
        /// <param name="token">Token a ser revogado.</param>
        /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
        public async Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            // Tenta encontrar o token no repositório
            var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);

            if (refreshToken == null)
                throw new TickestException("Refresh token não encontrado.", nameof(token));

            // Marca o token como revogado
            refreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
        }


        /// <summary>
        /// Marca um RefreshToken como usado.
        /// </summary>
        /// <param name="token">Token a ser marcado como usado.</param>
        /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
        public async Task MarkAsUsedAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(r => r.Token == token, cancellationToken);

            if (refreshToken == null)
                throw new TickestException("Refresh token não encontrado.", nameof(token));

            // Marca o token como usado
            refreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
        }

    }
}
