using System.Linq.Expressions;
using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório genérico que fornece operações básicas de CRUD
/// para entidades que herdam de <see cref="EntityBase{TKey}"/>.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade.</typeparam>
/// <typeparam name="TKey">Tipo da chave primária da entidade.</typeparam>
public interface IBaseRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    /// <summary>
    /// Verifica se algum registro existe que satisfaça o predicado fornecido.
    /// </summary>
    /// <param name="predicate">Expressão para filtrar os registros.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>True se algum registro satisfizer a condição; caso contrário, false.</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna o primeiro registro que satisfaça o predicado ou null caso não exista.
    /// </summary>
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna todos os registros da entidade.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna um registro pelo seu identificador.
    /// </summary>
    Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona um novo registro ao contexto.
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um registro existente.
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um registro pelo seu identificador.
    /// </summary>
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persiste as alterações realizadas no contexto de dados.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
