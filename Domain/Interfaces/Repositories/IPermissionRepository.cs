using Tickest.Domain.Entities.Permissions;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para o repositório de permissões. Herda de IGenericRepository para métodos básicos de CRUD.
/// </summary>
public interface IPermissionRepository : IBaseRepository<Permission>
{
    /// <summary>
    /// Obtém uma lista de permissões associadas a um usuário com base no seu ID.
    /// </summary>
    /// <param name="userId">ID do usuário cujas permissões serão recuperadas.</param>
    /// <returns>Uma lista de permissões associadas ao usuário.</returns>
    //Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(Guid userId);

    /// <summary>
    /// Obtém todas as permissões disponíveis no sistema.
    /// </summary>
    /// <returns>Uma lista de todas as permissões.</returns>
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();

    /// <summary>
    /// Obtém as permissões com base em uma lista de nomes de permissões.
    /// </summary>
    /// <param name="permissionNames">Uma coleção de nomes de permissões a serem recuperadas.</param>
    /// <param name="cancellationToken">O token de cancelamento da operação assíncrona.</param>
    /// <returns>Uma lista de permissões que correspondem aos nomes fornecidos.</returns>
    Task<IEnumerable<Permission>> GetPermissionsByNamesAsync(IEnumerable<string> permissionNames, CancellationToken cancellationToken);
}
