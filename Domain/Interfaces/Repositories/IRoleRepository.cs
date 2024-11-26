using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface que define os métodos para interação com a entidade Role.
/// </summary>
public interface IRoleRepository : IGenericRepository<Role>
{
    /// <summary>
    /// Obtém uma função (Role) pelo nome.
    /// </summary>
    /// <param name="name">Nome da função (Role).</param>
    /// <returns>Função correspondente ao nome fornecido.</returns>
    Task<Role> GetByNameAsync(string name);

    /// <summary>
    /// Obtém as funções (Roles) atribuídas a um usuário específico.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <returns>Lista de funções atribuídas ao usuário.</returns>
    Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);


    Task<IEnumerable<Role>> GetRolesByNamesAsync(IEnumerable<string> roleNames);
}
