using System.Linq.Expressions;
using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface genérica para repositórios de entidades, fornecendo métodos CRUD básicos.
/// </summary>
public interface IBaseRepository<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Encontra a primeira entidade que corresponde a uma expressão de pesquisa.
    /// </summary>
    /// <param name="predicate">Expressão para filtrar as entidades.</param>
    /// <param name="cancellationToken">Token de cancelamento para controlar a execução da operação.</param>
    /// <returns>Uma tarefa representando a operação assíncrona, com a entidade encontrada ou <c>null</c> se não houver correspondência.</returns>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera todos os itens da entidade.
    /// </summary>
    /// <param name="asNoTracking">Se true, o Entity Framework não rastreará as mudanças nas entidades recuperadas.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma lista de entidades.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Recupera um item da entidade pelo seu Id.
    /// </summary>
    /// <param name="id">O Id da entidade a ser recuperada.</param>
    /// <param name="asNoTracking">Se true, o Entity Framework não rastreará a entidade recuperada.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>A entidade correspondente ao Id fornecido ou null caso não encontrada.</returns>
    Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona um novo item à base de dados.
    /// </summary>
    /// <param name="entity">A entidade a ser adicionada.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma lista de itens à base de dados.
    /// </summary>
    /// <param name="entities">A lista de entidades a ser adicionada.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um item existente na base de dados.
    /// </summary>
    /// <param name="entity">A entidade a ser atualizada.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma lista de itens na base de dados.
    /// </summary>
    /// <param name="entities">A lista de entidades a ser atualizada.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um item específico da base de dados.
    /// </summary>
    /// <param name="entity">A entidade a ser removida.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um item da base de dados usando seu Id.
    /// </summary>
    /// <param name="id">O Id da entidade a ser removida.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove uma lista de itens da base de dados.
    /// </summary>
    /// <param name="entities">A lista de entidades a ser removida.</param>
    /// <param name="cancellationToken">Token de cancelamento para permitir a interrupção da operação.</param>
    /// <returns>Uma tarefa assíncrona.</returns>
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}
