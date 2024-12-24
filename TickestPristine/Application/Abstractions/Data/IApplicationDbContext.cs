using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Tickest.Domain.Entities.Users;

namespace Tickest.Application.Abstractions.Data;

/// <summary>
/// Interface que representa o contexto de dados da aplicação, fornecendo acesso ao DbSet de entidades e 
/// aos métodos necessários para persistir as alterações no banco de dados.
/// </summary>
public interface IApplicationDbContext : IDisposable
{
    /// <summary>
    /// Obtém o conjunto de dados (DbSet) para a entidade especificada.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <returns>Um DbSet para a entidade especificada.</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Obtém o DbSet para a entidade de usuário (User).
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Salva as alterações realizadas no contexto.
    /// </summary>
    /// <param name="cancellationToken">O token de cancelamento da operação.</param>
    /// <returns>O número de registros afetados.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtém a fachada do banco de dados, permitindo o acesso a operações de banco de dados em nível mais baixo.
    /// </summary>
    DatabaseFacade Database { get; }
}
