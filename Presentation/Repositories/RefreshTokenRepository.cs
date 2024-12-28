using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    private readonly TickestContext _context;

    public RefreshTokenRepository(TickestContext context) : base(context) =>
        _context = context;

    #region Métodos de Consulta

    // Método para buscar o refresh token usando um token específico
    public async Task<RefreshToken> GetByTokenAsync(string token,CancellationToken cancellationToken) =>
        await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsActive);

    // Método para buscar o usuário associado a um refresh token específico
    public async Task<User> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        // Busca o refresh token sem rastrear a entidade no contexto (o que é mais eficiente)
        var refreshTokenEntity = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (refreshTokenEntity is null)
            throw new TickestException("Refresh token não encontrado.");

        // Busca o usuário associado ao refresh token
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == refreshTokenEntity.UserId, cancellationToken);

        if (user is null)
            throw new TickestException("Usuário associado ao refresh token não encontrado.");

        return user;
    }


    #endregion
}
