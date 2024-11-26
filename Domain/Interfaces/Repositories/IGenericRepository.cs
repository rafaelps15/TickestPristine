using System.Linq.Expressions;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface genérica para operações de repositório, incluindo CRUD e consultas personalizadas.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade para operações do repositório.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Obtém todas as entidades do tipo especificado.
    /// </summary>
    /// <returns>Uma lista de entidades.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Obtém uma entidade pelo ID fornecido.
    /// </summary>
    /// <param name="id">O identificador único da entidade.</param>
    /// <returns>A entidade com o ID especificado.</returns>
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona uma nova entidade ao repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser adicionada.</param>
    /// <returns>A tarefa representando a operação assíncrona.</returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Atualiza uma entidade existente no repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser atualizada.</param>
    /// <returns>A tarefa representando a operação assíncrona.</returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Exclui uma entidade do repositório.
    /// </summary>
    /// <param name="entity">A entidade a ser excluída.</param>
    /// <returns>A tarefa representando a operação assíncrona.</returns>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Exclui uma entidade pelo ID fornecido.
    /// </summary>
    /// <param name="id">O identificador único da entidade a ser excluída.</param>
    /// <returns>A tarefa representando a operação assíncrona.</returns>
    Task DeleteByIdAsync(Guid id);

    /// <summary>
    /// Realiza uma consulta personalizada baseada em uma expressão de filtro.
    /// </summary>
    /// <param name="predicate">A expressão que define o critério de filtro.</param>
    /// <returns>Uma lista de entidades que atendem ao critério.</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Realiza uma consulta por descrição, se a entidade possuir uma propriedade "Description".
    /// </summary>
    /// <param name="description">A descrição a ser filtrada.</param>
    /// <returns>Uma lista de entidades que contêm a descrição especificada.</returns>
    Task<IEnumerable<TEntity>> FindByDescriptionAsync(string description);
}
