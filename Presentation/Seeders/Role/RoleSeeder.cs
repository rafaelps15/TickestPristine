using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.Persistence.Helpers;

namespace Tickest.Persistence.Seeders;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(
        TickestContext context,
        IApplicationSettingRepository applicationSettingRepository,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository)
    {
        // Verifica se as roles já foram semeadas antes de realizar a inserção
        await SeederHelper.SeedEntityIfNotExistAsync<Role>(
            context,
            applicationSettingRepository,
            "RolesSeeded",
            async () => await AddRolesAsync(roleRepository, permissionRepository)
        );
    }

    private static async Task AddRolesAsync(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        var permissionGroups = new List<PermissionGroup>
        {
            new AdminMasterPermissions(),
            new AdminGeneralPermissions(),
            new SectorAdminPermissions(),
            new DepartmentAdminPermissions(),
            new AreaAdminPermissions(),
            new TicketManagerPermissions(),
            new CollaboratorPermissions()
        };

        // Associa permissões às roles
        var roleData = new[]
        {
            ("AdminMaster", "Administrador do sistema com acesso total", permissionGroups.First(g => g.GetType() == typeof(AdminMasterPermissions))),
            ("AdminGeneral", "Administrador geral do sistema", permissionGroups.First(g => g.GetType() == typeof(AdminGeneralPermissions))),
            ("SectorAdmin", "Administrador de setor", permissionGroups.First(g => g.GetType() == typeof(SectorAdminPermissions))),
            ("DepartmentAdmin", "Administrador de departamento", permissionGroups.First(g => g.GetType() == typeof(DepartmentAdminPermissions))),
            ("AreaAdmin", "Administrador de área", permissionGroups.First(g => g.GetType() == typeof(AreaAdminPermissions))),
            ("TicketManager", "Gerente de tickets", permissionGroups.First(g => g.GetType() == typeof(TicketManagerPermissions))),
            ("Collaborator", "Colaborador", permissionGroups.First(g => g.GetType() == typeof(CollaboratorPermissions)))
        };

        foreach (var (roleName, description, permissionGroup) in roleData)
        {
            var role = await CreateRoleWithPermissionsAsync(roleRepository, permissionRepository, roleName, description, permissionGroup);
            await roleRepository.AddAsync(role);
        }
    }


    //Passar esse métod
    private static async Task<Role> CreateRoleWithPermissionsAsync(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        string roleName,
        string description,
        PermissionGroup permissionGroup)
    {
        var role = new Role
        {
            Name = roleName,
            Description = description
        };

        // Obtém as permissões associadas a essa role
        var permissions = permissionGroup.GetPermissions();
        role.RolePermissions = permissions.Select(permission => new RolePermission
        {
            Role = role,
            Permission = permission
        }).ToList();

        return role;
    }
}
