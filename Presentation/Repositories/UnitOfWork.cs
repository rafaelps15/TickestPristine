using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    #region - Campos Privados

    private readonly TickestContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IGenericRepository<TickestContext> _genericRepository;
    private bool _disposed;

    #endregion

    #region - Construtor

    public UnitOfWork(
        TickestContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IGenericRepository<TickestContext> genericRepository) =>
        (_context, _userRepository, _roleRepository, _genericRepository) =
        (context ?? throw new ArgumentNullException(nameof(context)),
         userRepository ?? throw new ArgumentNullException(nameof(userRepository)),
         roleRepository ?? throw new ArgumentNullException(nameof(roleRepository)),
         genericRepository ?? throw new ArgumentNullException(nameof(genericRepository)));

    #endregion

    #region - Métodos Públicos

    /// <summary>
    /// Obtém o repositório genérico para a entidade especificada.
    /// </summary>
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class =>
        (IGenericRepository<TEntity>)_genericRepository;

    /// <summary>
    /// Salva as alterações no banco de dados de forma assíncrona.
    /// </summary>
    public async Task<int> CommitAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            throw new TickestException("Erro ao tentar salvar as alterações no banco de dados.", dbEx);
        }
        catch (Exception ex)
        {
            throw new TickestException("Ocorreu um erro ao processar a transação.", ex);
        }
    }

    /// <summary>
    /// Aplica o filtro para considerar apenas entidades ativas.
    /// </summary>
    public IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class =>
        query.Where(e => EF.Property<bool>(e, "IsActive"));

    #endregion

    #region - Métodos Privados

    /// <summary>
    /// Libera os recursos utilizados pelo UnitOfWork.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _context?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this); // Impede o GC de finalizar a classe após Dispose ser chamado
    }

    #endregion

    #region - Propriedades de Repositórios

    /// <summary>
    /// Propriedade para acessar o repositório de usuários.
    /// </summary>
    public IUserRepository Users => _userRepository;

    /// <summary>
    /// Propriedade para acessar o repositório de roles.
    /// </summary>
    public IRoleRepository Roles => _roleRepository;

    #endregion
}
