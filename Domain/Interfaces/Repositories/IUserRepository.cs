using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para operações de repositório relacionadas aos usuários.
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Obtém um usuário pelo email.
    /// </summary>
    /// <param name="userEmail">O email do usuário.</param>
    /// <returns>O usuário correspondente ao email fornecido.</returns>
    Task<User> GetUserByEmailAsync(string userEmail);

    /// <summary>
    /// Obtém um usuário pelo nome.
    /// </summary>
    /// <param name="userName">O nome do usuário.</param>
    /// <returns>O usuário correspondente ao nome fornecido.</returns>
    Task<User?> GetByNameAsync(string userName);

    /// <summary>
    /// Obtém os papéis (roles) de um usuário.
    /// </summary>
    /// <param name="userId">O ID do usuário.</param>
    /// <returns>Uma lista de papéis associados ao usuário.</returns>
    Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId);

    /// <summary>
    /// Verifica se o email de um usuário já existe.
    /// </summary>
    /// <param name="userEmail">O email a ser verificado.</param>
    /// <returns>True se o email já existir, caso contrário False.</returns>
    Task<bool> DoesEmailExistAsync(string userEmail, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se existem usuários cadastrados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para a operação.</param>
    /// <returns>True se existirem usuários cadastrados, caso contrário False.</returns>
    Task<bool> AnyUsersExistAsync(CancellationToken cancellationToken);
}
