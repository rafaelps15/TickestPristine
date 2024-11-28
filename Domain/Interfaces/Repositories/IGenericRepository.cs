using System.Linq.Expressions;
using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface do repositório genérico para operações CRUD básicas e consultas personalizadas.
/// </summary>
/// <typeparam name="TEntity">O tipo da entidade gerenciada pelo repositório.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Recupera todas as entidades do tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona, contendo a lista de entidades.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Recupera uma entidade pelo seu ID.
    /// </summary>
    /// <param name="id">O ID da entidade a ser recuperada.</param>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade ou null se não encontrada.</returns>
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona uma nova entidade ao repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser adicionada.</param>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza uma entidade existente no repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser atualizada.</param>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui uma entidade existente do repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser excluída.</param>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Exclui uma entidade pelo seu ID do repositório.
    /// </summary>
    /// <param name="id">O ID da entidade a ser excluída.</param>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Encontra entidades com base em um predicado.
    /// </summary>
    /// <param name="predicate">O predicado para filtrar as entidades.</param>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona, contendo a lista de entidades que atendem ao predicado.</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Encontra entidades com base na propriedade 'Description'.
    /// </summary>
    /// <param name="description">A descrição para buscar.</param>
    /// <param name="cancellationToken">O token de cancelamento para observar enquanto aguarda a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona, contendo a lista de entidades com uma descrição correspondente.</returns>
    Task<IEnumerable<TEntity>> FindByDescriptionAsync(string description, CancellationToken cancellationToken);
}
