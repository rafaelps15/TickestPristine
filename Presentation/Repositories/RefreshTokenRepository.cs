using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        protected readonly TickestContext _context;

        public RefreshTokenRepository(TickestContext context) : base(context) =>
            _context = context;

        // Método para buscar o RefreshToken pelo token
        public async Task<RefreshToken> GetByTokenAsync(string token) =>
            await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsActive);

        // Método para buscar o usuário associado ao RefreshToken
        public async Task<User> GetByRefreshTokenAsync(string refreshToken)
        {
            // Encontrar o RefreshToken pelo token e verificar se está ativo
            var refreshTokenEntity = await _context.RefreshTokens
                .Where(rt => rt.Token == refreshToken && rt.IsActive)
                .Include(rt => rt.User)  // Usando o Include para carregar o User relacionado
                .FirstOrDefaultAsync();

            if (refreshTokenEntity == null)
                return null;

            // Retorna o usuário associado ao RefreshToken
            return refreshTokenEntity.User;  
        }
    }
}
