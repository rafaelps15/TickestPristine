//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;
//using Tickest.Domain.Entities;
//using Tickest.Domain.Interfaces.Repositories;
//using Tickest.Persistence.Data;

//namespace Tickest.Persistence.Repositories;

//public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
//{
//    private readonly TickestContext _context;

//    public UserRoleRepository(TickestContext context) : base(context) =>
//        _context = context;

//    public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken) =>

//             await _context.UserRoles
//            .Where(ur => ur.UserId == userId)
//            .Include(ur => ur.Role)
//            .ToListAsync(cancellationToken);

//    public async Task<bool> HasRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken) =>
//             await _context.UserRoles
//            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);


//    public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken) =>
//             await _context.UserRoles
//            .Where(ur => userIds.Contains(ur.UserId))
//            .Include(ur => ur.Role)
//            .ToListAsync(cancellationToken);

//    public async Task<IEnumerable<UserRole>> FindAsync(Expression<Func<UserRole, bool>> predicate, CancellationToken cancellationToken) =>
//             await _context.UserRoles
//            .Where(predicate)
//            .Include(ur => ur.Role)
//            .ToListAsync(cancellationToken);

//}
