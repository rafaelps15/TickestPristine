using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface que define os métodos específicos para o repositório de usuários.
/// Herda os métodos genéricos de CRUD do <see cref="IGenericRepository{User}"/>.
/// </summary>
public interface IUserRepository : IGenericRepository<User>
{
    /// <summary>
    /// Obtém um usuário com base no seu endereço de e-mail.
    /// </summary>
    /// <param name="userEmail">O endereço de e-mail do usuário.</param>
    /// <returns>O usuário encontrado, ou <c>null</c> se não encontrado.</returns>
    Task<User> GetUserByEmailAsync(string userEmail);

    /// <summary>
    /// Obtém um usuário com base no seu nome.
    /// </summary>
    /// <param name="userName">O nome do usuário.</param>
    /// <returns>O usuário encontrado, ou <c>null</c> se não encontrado.</returns>
    Task<User?> GetByNameAsync(string userName);

    /// <summary>
    /// Obtém os papéis de um usuário com base no seu ID.
    /// </summary>
    /// <param name="userId">O ID do usuário.</param>
    /// <returns>Uma lista de papéis atribuídos ao usuário.</returns>
    Task<IEnumerable<UserRole>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Verifica se já existe um usuário com o e-mail fornecido.
    /// </summary>
    /// <param name="userEmail">O endereço de e-mail a ser verificado.</param>
    /// <returns><c>true</c> se o e-mail já estiver em uso, <c>false</c> caso contrário.</returns>
    Task<bool> DoesEmailExistAsync(string userEmail);
}
