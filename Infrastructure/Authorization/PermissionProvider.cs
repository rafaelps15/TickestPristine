using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Interfaces.Repositories;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider : IPermissionProvider
{
    #region Campos Privados

    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ILogger<PermissionProvider> _logger;
    private readonly Dictionary<string, Func<HashSet<string>>> _rolePermissions;

    #endregion

    #region Construtor

    public PermissionProvider(
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        ILogger<PermissionProvider> logger)
        => (_userRepository,  _permissionRepository, _logger, _rolePermissions)
            = (userRepository,  permissionRepository, logger, InitializeRolePermissions());

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

        // Adiciona permissões atribuídas diretamente ao usuário
        var permissions = new HashSet<string>();

        var userPermissions = await GetPermissionsForUserDirectlyAsync(userId);
        permissions.UnionWith(userPermissions);

        return permissions;
    }

    private async Task<IEnumerable<string>> GetPermissionsForUserDirectlyAsync(Guid userId)
    {
        // A implementação do repositório de permissões do usuário seria chamada aqui.
        throw new NotImplementedException();
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
