using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

/// <summary>
/// Implementação da unidade de trabalho, fornecendo acesso aos repositórios e garantindo o controle de transações com o banco de dados.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    #region - Campos Privados

    private readonly TickestContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IGenericRepository<TickestContext> _genericRepository;
    private bool _disposed;

    #endregion

    #region - Construtor

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">Contexto do banco de dados.</param>
    /// <param name="userRepository">Repositório de usuários.</param>
    /// <param name="roleRepository">Repositório de roles.</param>
    /// <param name="userRoleRepository">Repositório de user roles.</param>
    /// <param name="genericRepository">Repositório genérico.</param>
    public UnitOfWork(
        TickestContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IGenericRepository<TickestContext> genericRepository) =>
        (_context, _userRepository, _roleRepository, _userRoleRepository, _genericRepository) =
        (context, userRepository, roleRepository, userRoleRepository, genericRepository);

    #endregion

    #region - Métodos Públicos

    /// <summary>
    /// Obtém o repositório genérico para a entidade especificada.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <returns>Repositório genérico para a entidade especificada.</returns>
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class =>
        (IGenericRepository<TEntity>)_genericRepository;

    /// <summary>
    /// Salva as alterações no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
    /// <returns>O número de registros afetados.</returns>
    /// <exception cref="TickestException">Lançado em caso de erro durante o commit da transação.</exception>
    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
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
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="query">A consulta que será filtrada.</param>
    /// <returns>A consulta filtrada para retornar apenas entidades ativas.</returns>
    public IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class =>
        query.Where(e => EF.Property<bool>(e, "IsActive"));

    #endregion

    #region - Métodos Privados

    /// <summary>
    /// Libera os recursos não gerenciados.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _context?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    #endregion

    #region - Propriedades de Repositórios

    /// <summary>
    /// Obtém o repositório de usuários.
    /// </summary>
    public IUserRepository Users => _userRepository;

    /// <summary>
    /// Obtém o repositório de roles.
    /// </summary>
    public IRoleRepository Roles => _roleRepository;

    /// <summary>
    /// Obtém o repositório de user roles.
    /// </summary>
    public IUserRoleRepository UserRoles => _userRoleRepository;

    /// <summary>
    /// Obtém o repositório genérico para o contexto Tickest.
    /// </summary>
    public IGenericRepository<TickestContext> GenericRepository => _genericRepository;

    #endregion
}
