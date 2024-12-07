//using Tickest.Domain.Entities;

//namespace Tickest.Domain.Interfaces.Repositories;

///// <summary>
///// Interface para o repositório de roles (papéis) do sistema.
///// Herda de IGenericRepository para operações básicas de CRUD.
///// </summary>
//public interface IRoleRepository : IGenericRepository<Role>
//{
//    /// <summary>
//    /// Obtém uma role pelo nome.
//    /// </summary>
//    /// <param name="name">Nome da role.</param>
//    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
//    /// <returns>Uma task com o resultado, contendo a role correspondente ao nome ou null.</returns>
//    Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken);

//    /// <summary>
//    /// Obtém todas as roles de um usuário baseado no seu ID.
//    /// </summary>
//    /// <param name="userId">ID do usuário.</param>
//    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
//    /// <returns>Uma lista de roles associadas ao usuário.</returns>
//    Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);

//    /// <summary>
//    /// Obtém a primeira role correspondente aos nomes fornecidos.
//    /// </summary>
//    /// <param name="roleNames">Lista de nomes das roles.</param>
//    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
//    /// <returns>A primeira role correspondente aos nomes fornecidos ou null.</returns>
//    Task<Role> GetFirstRoleByNamesAsync(string[] roleNames, CancellationToken cancellationToken);

//    /// <summary>
//    /// Obtém uma role correspondente aos nomes fornecidos.
//    /// </summary>
//    /// <param name="roleNames">Lista de nomes das roles.</param>
//    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
//    /// <returns>A role correspondente aos nomes fornecidos ou null.</returns>
//    Task<Role> GetRoleByNamesAsync(string[] roleNames, CancellationToken cancellationToken);

//    /// <summary>
//    /// Obtém todas as roles correspondentes aos nomes fornecidos.
//    /// </summary>
//    /// <param name="roleNames">Lista de nomes das roles.</param>
//    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
//    /// <returns>Uma lista contendo todas as roles correspondentes aos nomes fornecidos.</returns>
//    Task<IList<Role>> GetAllRolesByNamesAsync(IReadOnlyList<string> roleNames, CancellationToken cancellationToken);
//}
