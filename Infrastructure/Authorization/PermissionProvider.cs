using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Interfaces.Repositories;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider : IPermissionProvider
{
    #region Campos Privados

    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ILogger<PermissionProvider> _logger;
    private readonly Dictionary<string, Func<HashSet<string>>> _rolePermissions;

    #endregion

    #region Construtor

    /// <summary>
    /// Inicializa a classe PermissionProvider com os repositórios e logger.
    /// </summary>
    public PermissionProvider(
     IUserRepository userRepository,
     IRoleRepository roleRepository,
     IPermissionRepository permissionRepository,
     ILogger<PermissionProvider> logger)
     => (_userRepository, _roleRepository, _permissionRepository, _logger, _rolePermissions)
         = (userRepository, roleRepository, permissionRepository, logger, InitializeRolePermissions());

    #endregion

    #region Permissões por Papel

    /// <summary>
    /// Permissões do papel "AdminMaster" (Controle total do sistema).
    /// </summary>
    private static HashSet<string> GetMasterAdminPermissions() => new HashSet<string>
    {
        "FullSystemControl", "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments",
        "ManageAreas", "ManageTickets", "ViewReports", "AccessCriticalSettings"
    };

    /// <summary>
    /// Permissões do papel "GeneralAdmin" (Administrador geral com permissões sobre usuários, setores, departamentos e tickets).
    /// </summary>
    private static HashSet<string> GetGeneralAdminPermissions() => new HashSet<string>
    {
        "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments", "ManageAreas",
        "ManageTickets", "ViewReports"
    };

    /// <summary>
    /// Permissões do papel "SectorAdmin" (Administrador de setores).
    /// </summary>
    private static HashSet<string> GetSectorAdminPermissions() => new HashSet<string>
    {
        "ManageSectors", "ManageDepartments", "ManageAreas"
    };

    /// <summary>
    /// Permissões do papel "DepartmentAdmin" (Administrador de departamentos).
    /// </summary>
    private static HashSet<string> GetDepartmentAdminPermissions() => new HashSet<string>
    {
        "ManageDepartments", "ManageAreas", "AssignDepartmentRoles"
    };

    /// <summary>
    /// Permissões do papel "AreaAdmin" (Administrador de áreas).
    /// </summary>
    private static HashSet<string> GetAreaAdminPermissions() => new HashSet<string>
    {
        "ManageAreas", "ManageTasks", "ManageCollaborators"
    };

    /// <summary>
    /// Permissões do papel "TicketManager" (Responsável por gerenciar tickets).
    /// </summary>
    private static HashSet<string> GetTicketManagerPermissions() => new HashSet<string>
    {
        "ManageTickets", "ChangeTicketStatus", "ReassignTickets", "MonitorTicketPerformance"
    };

    /// <summary>
    /// Permissões do papel "Collaborator" (Colaborador que pode criar e rastrear tickets).
    /// </summary>
    private static HashSet<string> GetCollaboratorPermissions() => new HashSet<string>
    {
        "CreateTicket", "TrackTicketStatus", "InteractWithAnalyst"
    };

    /// <summary>
    /// Permissões do papel "SupportAnalyst" (Analista de suporte que gerencia tickets atribuídos e interage com solicitantes).
    /// </summary>
    private static HashSet<string> GetSupportAnalystPermissions() => new HashSet<string>
    {
        "ManageAssignedTickets", "UpdateTicketStatus", "InteractWithRequester"
    };

    #endregion

    // Inicialização de Permissões por Papel
    private Dictionary<string, Func<HashSet<string>>> InitializeRolePermissions()
    {
        return new Dictionary<string, Func<HashSet<string>>>
        {
            ["AdminMaster"] = GetMasterAdminPermissions,
            ["GeneralAdmin"] = GetGeneralAdminPermissions,
            ["SectorAdmin"] = GetSectorAdminPermissions,
            ["DepartmentAdmin"] = GetDepartmentAdminPermissions,
            ["AreaAdmin"] = GetAreaAdminPermissions,
            ["TicketManager"] = GetTicketManagerPermissions,
            ["Collaborator"] = GetCollaboratorPermissions,
            ["SupportAnalyst"] = GetSupportAnalystPermissions
        };
    }

    /// <summary>
    /// Permissões de exemplo para os papéis:
    /// <list type="bullet">
    ///     <item><description>FullSystemControl – Controle total do sistema</description></item>
    ///     <item><description>ManageUsers – Gerenciar usuários</description></item>
    ///     <item><description>ManagePermissions – Gerenciar permissões</description></item>
    ///     <item><description>ManageSectors – Gerenciar setores</description></item>
    ///     <item><description>ManageDepartments – Gerenciar departamentos</description></item>
    ///     <item><description>ManageAreas – Gerenciar áreas</description></item>
    ///     <item><description>ManageTickets – Gerenciar tickets</description></item>
    ///     <item><description>ViewReports – Visualizar relatórios</description></item>
    ///     <item><description>AccessCriticalSettings – Acessar configurações críticas</description></item>
    ///     <item><description>AssignDepartmentRoles – Atribuir funções de departamento</description></item>
    ///     <item><description>ManageTasks – Gerenciar tarefas</description></item>
    ///     <item><description>ManageCollaborators – Gerenciar colaboradores</description></item>
    ///     <item><description>ChangeTicketStatus – Alterar status do ticket</description></item>
    ///     <item><description>ReassignTickets – Reatribuir tickets</description></item>
    ///     <item><description>MonitorTicketPerformance – Monitorar o desempenho do ticket</description></item>
    ///     <item><description>CreateTicket – Criar ticket</description></item>
    ///     <item><description>TrackTicketStatus – Rastrear status do ticket</description></item>
    ///     <item><description>InteractWithAnalyst – Interagir com o analista</description></item>
    ///     <item><description>ManageAssignedTickets – Gerenciar tickets atribuídos</description></item>
    ///     <item><description>UpdateTicketStatus – Atualizar status do ticket</description></item>
    ///     <item><description>InteractWithRequester – Interagir com o solicitante</description></item>
    /// </list>
    /// </summary>
    public async Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId)
    {
        var roles = await _userRepository.GetUserRolesAsync(userId);
        var permissions = new HashSet<string>();

        // Add permissions by role
        foreach (var role in roles)
        {
            if (_rolePermissions.TryGetValue(role.Role.Name, out var permissionFunc))
            {
                permissions.UnionWith(permissionFunc());
            }
            else
            {
                _logger.LogWarning($"Unknown role: {role.Role.Name}");
            }
        }

        // Add permissions assigned directly to the user
        var userPermissions = await GetPermissionsForUserDirectlyAsync(userId);
        permissions.UnionWith(userPermissions);

        return permissions;
    }

    private async Task<HashSet<string>> GetPermissionsForUserDirectlyAsync(Guid userId)
    {
        // Obtém as permissões do usuário via repositório
        var userPermissions = await _permissionRepository.GetPermissionsByUserIdAsync(userId);

        // Se não encontrar permissões, retornar um HashSet vazio
        if (userPermissions == null || !userPermissions.Any())
        {
            return new HashSet<string>();
        }

        // Retorna um HashSet com os nomes das permissões
        return new HashSet<string>(userPermissions.Select(up => up.Name));
    }


    public HashSet<string> GetPermissionsForRole(string roleName)
    {
        if (_rolePermissions.ContainsKey(roleName))
        {
            return _rolePermissions[roleName]();
        }
        return new HashSet<string>(); // Retorna um conjunto vazio caso a role não seja encontrada
    }


}
