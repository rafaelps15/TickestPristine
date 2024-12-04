using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    protected readonly TickestContext _context;

    public RefreshTokenRepository(TickestContext context) : base(context) =>
        _context = context;

    #region Métodos de Consulta

    public async Task<RefreshToken> GetByTokenAsync(string token) =>
        await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsActive);

    public async Task<User> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (refreshTokenEntity == null)
            throw new TickestException("Refresh token não encontrado.");

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == refreshTokenEntity.UserId, cancellationToken);

        if (user == null)
            throw new TickestException("Usuário associado ao refresh token não encontrado.");

        return user;
    }

    #endregion
}
