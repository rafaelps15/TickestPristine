using System.Linq.Expressions;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface genérica para repositório base que define operações comuns de persistência.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade que herda de EntityBase.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Verifica se existe algum registro que satisfaz o predicado informado.
    /// </summary>
    /// <param name="predicate">Expressão para filtro da consulta.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>True se existir pelo menos um registro, caso contrário, false.</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Busca a primeira entidade que satisfaz o predicado informado, ou null caso não encontre.
    /// </summary>
    /// <param name="predicate">Expressão para filtro da consulta.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Entidade encontrada ou null.</returns>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Retorna todas as entidades do tipo informado.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Lista com todas as entidades.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca a entidade pelo seu identificador único (chave primária).
    /// </summary>
    /// <param name="id">Identificador único da entidade.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Entidade encontrada ou null.</returns>
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade no contexto e salva as alterações.
    /// </summary>
    /// <param name="entity">Entidade a ser adicionada.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma coleção de entidades no contexto e salva as alterações.
    /// </summary>
    /// <param name="entities">Coleção de entidades a serem adicionadas.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente no contexto e salva as alterações.
    /// </summary>
    /// <param name="entity">Entidade a ser atualizada.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma coleção de entidades existentes no contexto e salva as alterações.
    /// </summary>
    /// <param name="entities">Coleção de entidades a serem atualizadas.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a entidade pelo seu identificador único e salva as alterações.
    /// </summary>
    /// <param name="id">Identificador único da entidade a ser removida.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persiste as alterações feitas no contexto no banco de dados.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
