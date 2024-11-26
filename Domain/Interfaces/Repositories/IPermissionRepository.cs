using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface que define as operações de repositório para a entidade Permission.
/// Herda os métodos genéricos de CRUD do <see cref="IGenericRepository{Permission}"/>.
/// </summary>
public interface IPermissionRepository : IGenericRepository<Permission>
{
    /// <summary>
    /// Obtém as permissões atribuídas a um usuário específico.
    /// </summary>
    /// <param name="userId">O ID do usuário cujas permissões serão retornadas.</param>
    /// <returns>Uma lista de permissões do usuário.</returns>
    Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(Guid userId);

    /// <summary>
    /// Obtém todas as permissões do sistema.
    /// </summary>
    /// <returns>Uma lista de todas as permissões disponíveis no sistema.</returns>
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
}
