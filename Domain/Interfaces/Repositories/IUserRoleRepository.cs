using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para repositório de associação entre usuários e roles.
/// </summary>
public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    /// <summary>
    /// Obtém todas as associações de roles para um usuário específico.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <returns>Uma lista de associações de roles do usuário.</returns>
    Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(Guid userId);

    /// <summary>
    /// Verifica se um usuário possui uma role específica.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <param name="roleId">ID da role.</param>
    /// <returns>True se o usuário possui a role; caso contrário, false.</returns>
    Task<bool> HasRoleAsync(Guid userId, Guid roleId);

    /// <summary>
    /// Obtém todas as roles atribuídas a um conjunto de usuários.
    /// </summary>
    /// <param name="userIds">Lista de IDs de usuários.</param>
    /// <returns>Uma lista de associações entre usuários e roles.</returns>
    Task<IEnumerable<UserRole>> GetUserRolesByUserIdsAsync(IEnumerable<Guid> userIds);
}
