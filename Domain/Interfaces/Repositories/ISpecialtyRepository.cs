using Tickest.Domain.Entities.Specialties;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface que define as operações específicas para a entidade <see cref="Specialty"/>
/// e herda operações genéricas do repositório <see cref="IGenericRepository{T}"/>.
/// </summary>
public interface ISpecialtyRepository : IGenericRepository<Specialty>
{
    /// <summary>
    /// Método assíncrono para buscar especialidades com base em uma lista de nomes.
    /// </summary>
    /// <param name="specialtyNames">Coleção de nomes de especialidades a serem buscadas.</param>
    /// <param name="cancellationToken">Token de cancelamento para controle de operações assíncronas.</param>
    /// <returns>Uma coleção de objetos <see cref="Specialty"/> que correspondem aos nomes fornecidos.</returns>
    /// <exception cref="TickestException">Lança uma exceção se nenhuma especialidade for encontrada.</exception>
    Task<ICollection<Specialty>> GetSpecialtiesByNamesAsync(IEnumerable<string> specialtyNames, CancellationToken cancellationToken);
    Task<IEnumerable<object>> GetSpecialtiesByNamesAsync(IReadOnlyList<Guid> specialtyNames, CancellationToken cancellationToken);
}
