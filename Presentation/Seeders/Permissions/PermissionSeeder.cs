//using Tickest.Domain.Entities.Permissions;
//using Tickest.Domain.Entities;
//using Tickest.Domain.Interfaces.Repositories;
//using Tickest.Persistence.Data;

//public class PermissionSeeder
//{
//    private readonly TickestContext _context;
//    private readonly IPermissionRepository _permissionRepository;
//    private readonly IApplicationSettingRepository _applicationSettingRepository;

//    public PermissionSeeder(IPermissionRepository permissionRepository, IApplicationSettingRepository applicationSettingRepository)
//    {
//        _permissionRepository = permissionRepository;
//        _applicationSettingRepository = applicationSettingRepository;
//    }

//    public async Task SeedPermissionsAsync(CancellationToken cancellationToken = default)
//    {
//        // Verifica se as permissões já foram semeadas
//        var seederFlag = await _applicationSettingRepository.GetSettingAsync("PermissionsSeeded");
//        if (seederFlag?.Value == "True") return;

//        // Adiciona permissões predefinidas, evitando duplicação
//        var allPermissions = GetAllPermissions();
//        foreach (var permission in allPermissions)
//        {
//            if (!await _permissionRepository.AnyAsync(p => p.Name == permission.Name, cancellationToken))
//            {
//                await _permissionRepository.AddAsync(permission, cancellationToken);
//            }
//        }

//        // Atualiza ou insere o estado do seed no banco
//        await _applicationSettingRepository.SetSettingAsync(
//            new ApplicationSetting { Key = "PermissionsSeeded", Value = "True" }
//        );
//    }

//    // Métodos de Permissões
//    private static List<Permission> GetPermissions(string role, params string[] permissions)
//        => permissions.Select(p => new Permission { Name = $"{role}.{p}" }).ToList();

//    public List<Permission> GetAdminMasterPermissions()
//        => GetPermissions("AdminMaster", "FullAccess", "ManageUsers", "ManageRoles", "ManageSystemSettings", "AccessChat", "ViewAuditLogs");

//    public List<Permission> GetAdminGeneralPermissions()
//        => GetPermissions("AdminGeneral", "FullAccess", "ManageUsers", "ViewTickets", "ManageTickets", "AccessChat", "ManageDepartments", "ManageAreas", "ManageSectors");

//    public List<Permission> GetSectorAdminPermissions()
//        => GetPermissions("SectorAdmin", "ManageDepartments", "ViewSectorTickets", "ManageSectorAreas", "AccessChat");

//    public List<Permission> GetDepartmentAdminPermissions()
//        => GetPermissions("DepartmentAdmin", "ManageDepartment", "ViewDepartmentTickets", "ManageDepartmentAreas", "AccessChat");

//    public List<Permission> GetAreaAdminPermissions()
//        => GetPermissions("AreaAdmin", "ManageArea", "ViewAreaTickets", "ManageUsersInArea", "AccessChat");

//    public List<Permission> GetTicketManagerPermissions()
//        => GetPermissions("TicketManager", "AssignTicketsToSpecialists", "ViewAllTickets", "ViewTicketDetails", "VerifyUserData", "ManageTickets", "AssignTicketsToOthers", "ViewTicketHistory", "AccessChat");

//    public List<Permission> GetCollaboratorPermissions()
//        => GetPermissions("Collaborator", "ViewTickets", "EditTickets", "Finalize", "SendTicketMessage", "AccessChat");

//    private List<Permission> GetAllPermissions()
//    {
//        var permissions = new List<Permission>();
//        permissions.AddRange(GetAdminMasterPermissions());
//        permissions.AddRange(GetAdminGeneralPermissions());
//        permissions.AddRange(GetSectorAdminPermissions());
//        permissions.AddRange(GetDepartmentAdminPermissions());
//        permissions.AddRange(GetAreaAdminPermissions());
//        permissions.AddRange(GetTicketManagerPermissions());
//        permissions.AddRange(GetCollaboratorPermissions());
//        return permissions;
//    }
//}
