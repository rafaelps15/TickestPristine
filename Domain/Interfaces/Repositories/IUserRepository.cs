using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para interagir com os dados relacionados ao usuário no repositório.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Obtém um usuário pelo endereço de e-mail.
    /// </summary>
    /// <param name="userEmail">O endereço de e-mail do usuário a ser recuperado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a entidade do usuário.</returns>
    Task<User> GetUserByEmailAsync(string userEmail);

    /// <summary>
    /// Verifica se um usuário com o e-mail fornecido já existe no banco de dados.
    /// </summary>
    /// <param name="userEmail">O endereço de e-mail a ser verificado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém verdadeiro se o e-mail existir, caso contrário, falso.</returns>
    Task<bool> DoesEmailExistAsync(string userEmail);

    ///// <summary>
    ///// Obtém os papéis de um usuário juntamente com suas permissões associadas.
    ///// </summary>
    ///// <param name="userId">O ID do usuário para o qual os papéis serão recuperados.</param>
    ///// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma lista de papéis do usuário com suas permissões.</returns>
    //Task<List<UserRole>> GetUserRolesAsync(Guid userId);

    ///// <summary>
    ///// Obtém os nomes dos papéis atribuídos a um usuário.
    ///// </summary>
    ///// <param name="userId">O ID do usuário para o qual os nomes dos papéis serão recuperados.</param>
    ///// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma lista com os nomes dos papéis.</returns>
    //Task<IEnumerable<string>> GetUserRoleNamesAsync(Guid userId);

    ///// <summary>
    ///// Verifica se um usuário possui uma permissão específica atribuída por meio de seus papéis.
    ///// </summary>
    ///// <param name="userId">O ID do usuário para o qual a permissão será verificada.</param>
    ///// <param name="permission">O nome da permissão a ser verificada.</param>
    ///// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém verdadeiro se o usuário possuir a permissão, caso contrário, falso.</returns>
    //Task<bool> UserHasPermissionAsync(Guid userId, string permission);
}
