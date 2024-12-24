//using Tickest.Domain.Entities;
//using System.Linq.Expressions;

//namespace Tickest.Domain.Interfaces.Repositories;

///// <summary>
///// Interface para repositório de associação entre usuários e roles.
///// </summary>
//public interface IUserRoleRepository : IGenericRepository<UserRole>
//{
//    /// <summary>
//    /// Obtém todas as associações de roles para um usuário específico.
//    /// </summary>
//    Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);

//    /// <summary>
//    /// Verifica se um usuário possui uma role específica.
//    /// </summary>
//    Task<bool> HasRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);

//    /// <summary>
//    /// Obtém todas as roles atribuídas a um conjunto de usuários.
//    /// </summary>
//    Task<IEnumerable<UserRole>> GetUserRolesByUserIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken);

//    /// <summary>
//    /// Realiza uma consulta personalizada com base em uma expressão de filtro.
//    /// </summary>
//    Task<IEnumerable<UserRole>> FindAsync(Expression<Func<UserRole, bool>> predicate, CancellationToken cancellationToken);
//}
