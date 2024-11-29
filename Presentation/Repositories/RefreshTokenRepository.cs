//using Microsoft.EntityFrameworkCore;
//using Tickest.Domain.Entities;
//using Tickest.Domain.Interfaces.Repositories;
//using Tickest.Persistence.Data;

//namespace Tickest.Persistence.Repositories;

//public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
//{
//    protected readonly TickestContext _context;

//    public RefreshTokenRepository(TickestContext context) : base(context) =>
//        _context = context;

//    #region Métodos de Consulta

//    public async Task<RefreshToken> GetByTokenAsync(string token) =>
//        await _context.RefreshTokens
//            .FirstOrDefaultAsync(rt => rt.Token == token && rt.IsActive);

//    public async Task<User> GetByRefreshTokenAsync(string refreshToken)
//    {
//        var refreshTokenEntity = await _context.RefreshTokens
//            .Where(rt => rt.Token == refreshToken && rt.IsActive)
//            .Include(rt => rt.User)
//            .FirstOrDefaultAsync();

//        if (refreshTokenEntity == null)
//            return null;

//        return refreshTokenEntity.User;
//    }

//    #endregion
//}
