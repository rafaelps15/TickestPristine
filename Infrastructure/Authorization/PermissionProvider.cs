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

    public PermissionProvider(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        ILogger<PermissionProvider> logger)
        => (_userRepository, _roleRepository, _permissionRepository, _logger, _rolePermissions)
            = (userRepository, roleRepository, permissionRepository, logger, InitializeRolePermissions());

    #endregion

    #region Permissões por Papel

    private static HashSet<string> GetMasterAdminPermissions() => new HashSet<string>
    {
        "FullSystemControl", "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments",
        "ManageAreas", "ManageTickets", "ViewReports", "AccessCriticalSettings"
    };

    private static HashSet<string> GetGeneralAdminPermissions() => new HashSet<string>
    {
        "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments", "ManageAreas",
        "ManageTickets", "ViewReports"
    };

    private static HashSet<string> GetSectorAdminPermissions() => new HashSet<string>
    {
        "ManageSectors", "ManageDepartments", "ManageAreas"
    };

    private static HashSet<string> GetDepartmentAdminPermissions() => new HashSet<string>
    {
        "ManageDepartments", "ManageAreas", "AssignDepartmentRoles"
    };

    private static HashSet<string> GetAreaAdminPermissions() => new HashSet<string>
    {
        "ManageAreas", "ManageTasks", "ManageCollaborators"
    };

    private static HashSet<string> GetTicketManagerPermissions() => new HashSet<string>
    {
        "ManageTickets", "ChangeTicketStatus", "ReassignTickets", "MonitorTicketPerformance"
    };

    private static HashSet<string> GetCollaboratorPermissions() => new HashSet<string>
    {
        "CreateTicket", "TrackTicketStatus", "InteractWithAnalyst"
    };

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
    public async Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do usuário não pode ser vazio.", nameof(userId));

        // Obtém as funções associadas ao usuário
        var roles = await _userRepository.GetUserRolesAsync(userId);
        if (roles == null || !roles.Any())
            return new HashSet<string>(); // Nenhuma permissão encontrada para o usuário

        var permissions = new HashSet<string>();

        // Adiciona permissões baseadas nos papéis do usuário
        foreach (var role in roles)
        {
            if (!string.IsNullOrWhiteSpace(role.Role?.Name))
            {
                var rolePermissions = GetPermissionsForRole(role.Role.Name);
                if (rolePermissions != null)
                    permissions.UnionWith(rolePermissions);
            }
        }

        // Adiciona permissões atribuídas diretamente ao usuário
        var userPermissions = await GetPermissionsForUserDirectlyAsync(userId);
        if (userPermissions != null)
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
        // Verifica se o papel está no dicionário de permissões
        if (_rolePermissions.ContainsKey(roleName))
        {
            return _rolePermissions[roleName](); // Chama o método associado
        }

        // Caso o papel não seja encontrado no dicionário, aplica o switch expression para verificar o papel
        return roleName switch
        {
            "AdminMaster" => GetMasterAdminPermissions(),
            "GeneralAdmin" => GetGeneralAdminPermissions(),
            "SectorAdmin" => GetSectorAdminPermissions(),
            "DepartmentAdmin" => GetDepartmentAdminPermissions(),
            "AreaAdmin" => GetAreaAdminPermissions(),
            "TicketManager" => GetTicketManagerPermissions(),
            "Collaborator" => GetCollaboratorPermissions(),
            "SupportAnalyst" => GetSupportAnalystPermissions(),
            _ => new HashSet<string>() // Retorna um conjunto vazio se o papel não for reconhecido
        };
    }

    // Adicionando um método de verificação de permissões por nome
    public async Task<bool> UserHasPermissionAsync(Guid userId, string permission)
    {
        // Verifica se o usuário tem a permissão
        var userPermissions = await GetPermissionsForUserAsync(userId);
        return userPermissions.Contains(permission);
    }
}
