using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface que define os métodos de acesso aos dados dos usuários, estendendo os métodos genéricos para manipulação da entidade User.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Obtém um usuário pelo seu e-mail.
    /// </summary>
    /// <param name="userEmail">O e-mail do usuário a ser buscado.</param>
    /// <returns>O usuário correspondente ao e-mail fornecido.</returns>
    Task<User?> GetUserByEmailAsync(string userEmail,CancellationToken cancellationToken);

    /// <summary>
    /// Obtém um usuário pelo seu nome.
    /// </summary>
    /// <param name="userName">O nome do usuário a ser buscado.</param>
    /// <returns>O usuário correspondente ao nome fornecido.</returns>
    Task<User?> GetByNameAsync(string userName);

    /// <summary>
    /// Obtém as funções associadas a um usuário.
    /// </summary>
    /// <param name="userId">O identificador do usuário.</param>
    /// <returns>Uma lista das funções associadas ao usuário.</returns>
    //Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId);

    /// <summary>
    /// Verifica se já existe um usuário com o e-mail fornecido.
    /// </summary>
    /// <param name="userEmail">O e-mail a ser verificado.</param>
    /// <param name="cancellationToken">O token de cancelamento para operações assíncronas.</param>
    /// <returns>Retorna verdadeiro se o e-mail já estiver em uso, falso caso contrário.</returns>
    //Task<bool> DoesEmailExistAsync(string userEmail, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se existem usuários cadastrados.
    /// </summary>
    /// <param name="cancellationToken">O token de cancelamento para operações assíncronas.</param>
    /// <returns>Retorna verdadeiro se existir pelo menos um usuário, falso caso contrário.</returns>
    Task<bool> AnyUsersExistAsync(CancellationToken cancellationToken);
    
}
