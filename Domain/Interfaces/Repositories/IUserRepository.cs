using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface responsável pelas operações de persistência de dados relacionadas ao usuário.
    /// </summary>
    public interface IUserRepository:IBaseRepository<User>
    {
        /// <summary>
        /// Obtém um usuário pelo seu e-mail.
        /// </summary>
        /// <param name="userEmail">E-mail do usuário.</param>
        /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
        /// <returns>Um usuário ou null, se não encontrado.</returns>
        Task<User?> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken);

        /// <summary>
        /// Obtém um usuário pelo seu nome.
        /// </summary>
        /// <param name="userName">Nome do usuário.</param>
        /// <returns>Um usuário ou null, se não encontrado.</returns>
        Task<User?> GetByNameAsync(string userName);

        /// <summary>
        /// Verifica se um usuário com o e-mail informado já existe no banco de dados.
        /// </summary>
        /// <param name="userEmail">E-mail do usuário a ser verificado.</param>
        /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
        /// <returns>True se o e-mail já existir; caso contrário, false.</returns>
        Task<bool> DoesEmailExistAsync(string userEmail, CancellationToken cancellationToken);

        /// <summary>
        /// Verifica se existe pelo menos um usuário no banco de dados.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
        /// <returns>True se existir pelo menos um usuário; caso contrário, false.</returns>
        Task<bool> AnyUsersExistAsync(CancellationToken cancellationToken);

        Task<Role> GetUserRoleAsync(User currentUser, CancellationToken cancellationToken);
     
    }
}
