using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;
using Tickest.Persistence.Helpers;

namespace Tickest.Persistence.Seeders;

public static class PermissionSeeder
{
    // Verifica se as permissões foram semeadas, se não, executa o processo de seed
    public static async Task SeedPermissionsAsync(
        TickestContext context,
        IApplicationSettingRepository applicationSettingRepository,
        IPermissionRepository permissionRepository)
    {
        await SeederHelper.SeedEntityIfNotExistAsync<Permission>(
            context,
            applicationSettingRepository,
            "PermissionsSeeded",
            async () => await AddPermissionsAsync(permissionRepository)
        );
    }

    // Método que adiciona as permissões ao banco
    private static async Task AddPermissionsAsync(IPermissionRepository permissionRepository)
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

        foreach (var permissionGroup in permissionGroups)
        {
            var permissions = permissionGroup.GetPermissions();
            await permissionRepository.AddRangeAsync(permissions);
        }
    }
}

// Abstração para representar um grupo de permissões
public abstract class PermissionGroup
{
    public abstract IEnumerable<Permission> GetPermissions();
}

#region Implementações específicas dos grupos de permissões

public class AdminMasterPermissions : PermissionGroup
{
    public override IEnumerable<Permission> GetPermissions() =>
        new List<Permission>
        {
            new Permission { Name = "AdminMaster.FullAccess", Description = "Acesso total ao sistema" },
            new Permission { Name = "AdminMaster.ManageUsers", Description = "Gerenciar usuários" },
            new Permission { Name = "AdminMaster.ManageRoles", Description = "Gerenciar funções de usuário" },
            new Permission { Name = "AdminMaster.ManageSystemSettings", Description = "Gerenciar configurações do sistema" },
            new Permission { Name = "AdminMaster.AccessChat", Description = "Acesso ao chat" },
            new Permission { Name = "AdminMaster.ViewAuditLogs", Description = "Visualizar logs de auditoria" }
        };
}

public class AdminGeneralPermissions : PermissionGroup
{
    public override IEnumerable<Permission> GetPermissions() =>
        new List<Permission>
        {
            new Permission { Name = "AdminGeneral.FullAccess", Description = "Acesso total ao sistema" },
            new Permission { Name = "AdminGeneral.ManageUsers", Description = "Gerenciar usuários" },
            new Permission { Name = "AdminGeneral.ViewTickets", Description = "Visualizar tickets" },
            new Permission { Name = "AdminGeneral.ManageTickets", Description = "Gerenciar tickets" },
            new Permission { Name = "AdminGeneral.AccessChat", Description = "Acesso ao chat" },
            new Permission { Name = "AdminGeneral.ManageDepartments", Description = "Gerenciar departamentos" },
            new Permission { Name = "AdminGeneral.ManageAreas", Description = "Gerenciar áreas" },
            new Permission { Name = "AdminGeneral.ManageSectors", Description = "Gerenciar setores" }
        };
}

public class SectorAdminPermissions : PermissionGroup
{
    public override IEnumerable<Permission> GetPermissions() =>
        new List<Permission>
        {
            new Permission { Name = "SectorAdmin.ManageDepartments", Description = "Gerenciar departamentos" },
            new Permission { Name = "SectorAdmin.ViewSectorTickets", Description = "Visualizar tickets do setor" },
            new Permission { Name = "SectorAdmin.ManageSectorAreas", Description = "Gerenciar áreas do setor" },
            new Permission { Name = "SectorAdmin.AccessChat", Description = "Acesso ao chat" }
        };
}

public class DepartmentAdminPermissions : PermissionGroup
{
    public override IEnumerable<Permission> GetPermissions() =>
        new List<Permission>
        {
            new Permission { Name = "DepartmentAdmin.ManageDepartment", Description = "Gerenciar departamento" },
            new Permission { Name = "DepartmentAdmin.ViewDepartmentTickets", Description = "Visualizar tickets do departamento" },
            new Permission { Name = "DepartmentAdmin.ManageDepartmentAreas", Description = "Gerenciar áreas do departamento" },
            new Permission { Name = "DepartmentAdmin.AccessChat", Description = "Acesso ao chat" }
        };
}

public class AreaAdminPermissions : PermissionGroup
{
    public override IEnumerable<Permission> GetPermissions() =>
        new List<Permission>
        {
            new Permission { Name = "AreaAdmin.ManageArea", Description = "Gerenciar área" },
            new Permission { Name = "AreaAdmin.ViewAreaTickets", Description = "Visualizar tickets da área" },
            new Permission { Name = "AreaAdmin.ManageUsersInArea", Description = "Gerenciar usuários da área" },
            new Permission { Name = "AreaAdmin.AccessChat", Description = "Acesso ao chat" }
        };
}

public class TicketManagerPermissions : PermissionGroup
{
    public override IEnumerable<Permission> GetPermissions() =>
        new List<Permission>
        {
            new Permission { Name = "TicketManager.AssignTicketsToSpecialists", Description = "Atribuir tickets a especialistas" },
            new Permission { Name = "TicketManager.ViewAllTickets", Description = "Visualizar todos os tickets" },
            new Permission { Name = "TicketManager.ViewTicketDetails", Description = "Visualizar detalhes de tickets" },
            new Permission { Name = "TicketManager.VerifyUserData", Description = "Verificar dados de usuários" },
            new Permission { Name = "TicketManager.ManageTickets", Description = "Gerenciar tickets" },
            new Permission { Name = "TicketManager.AssignTicketsToOthers", Description = "Atribuir tickets a outros usuários" },
            new Permission { Name = "TicketManager.ViewTicketHistory", Description = "Visualizar histórico de tickets" },
            new Permission { Name = "TicketManager.AccessChat", Description = "Acesso ao chat" }
        };
}

public class CollaboratorPermissions : PermissionGroup
{
    public override IEnumerable<Permission> GetPermissions() =>
        new List<Permission>
        {
            new Permission { Name = "Collaborator.ViewTickets", Description = "Visualizar tickets" },
            new Permission { Name = "Collaborator.EditTickets", Description = "Editar tickets" },
            new Permission { Name = "Collaborator.Finalize", Description = "Finalizar tickets" },
            new Permission { Name = "Collaborator.SendTicketMessage", Description = "Enviar mensagens para o ticket" },
            new Permission { Name = "Collaborator.AccessChat", Description = "Acesso ao chat" }
        };
}

#endregion
