using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Persistence.Repositories;

public sealed class RefreshTokenRepository(ApplicationDbContext context)
    : Repository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken) =>
        await DbSet
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsActive, cancellationToken)
        ?? throw new TickestException("Refresh token não encontrado.");

    public async Task<User> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (refreshTokenEntity is null)
        {
            throw new TickestException("Refresh token não encontrado.");
        }

        var user = await DbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == refreshTokenEntity.UserId, cancellationToken);

        return user ?? throw new TickestException("Usuário associado ao refresh token não encontrado.");
    }
}
