using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using static Tickest.Persistence.Seeders.PermissionSeeder;

namespace Tickest.Persistence.Seeders;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(
        TickestContext context,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository = null)
    {
        await AddRolesAsync(context, roleRepository, permissionRepository);
    }

    private static async Task AddRolesAsync(TickestContext context, IRoleRepository roleRepository, IPermissionRepository permissionRepository)
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

        var allPermissions = permissionGroups.SelectMany(group => group.GetPermissions()).ToList();

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
            var role = new Role
            {
                Name = roleName,
                Description = description,
                CreatedAt = DateTime.Now,
                IsActive = true,
                DeactivatedAt = null,
                UpdateAt = null
            };

            foreach (var permission in permissionGroup.GetPermissions())
            {
                // Verifica se a permissão já existe
                var existingPermission = await permissionRepository.GetPermissionByNameAsync(permission.Name);

                if (existingPermission != null)
                {
                    // Associar a permissão à role
                    var rolePermission = new RolePermission
                    {
                        Role = role,
                        Permission = existingPermission
                    };

                    // Adiciona a associação RolePermission à role
                    role.RolePermissions.Add(rolePermission);
                }
            }

            await roleRepository.AddAsync(role);
        }
    }
}
