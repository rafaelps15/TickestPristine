using Tickest.Domain.Entities.Permissions;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public class RoleSeeder
{
    private readonly TickestContext _context;

    public RoleSeeder(TickestContext context)
    {
        _context = context;
    }

    public void SeedRoles()
    {
        if (!_context.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role{ Name = "AdminMaster", Description = "Controle total do Sistema", Permissions = GetMasterAdminPermissions() },
                new Role{ Name = "GeneralAdmin", Description = "Administração geral do sistema", Permissions = GetGeneralAdminPermissions() },
                new Role{ Name = "SectorAdmin", Description = "Administração do setor", Permissions = GetSectorAdminPermissions() },
                new Role{ Name = "DepartmentAdmin", Description = "Administração do departamento", Permissions = GetDepartmentAdminPermissions() },
                new Role{ Name = "AreaAdmin", Description = "Administração da área", Permissions = GetAreaAdminPermissions() },
                new Role{ Name = "TicketManager", Description = "Gestão de tickets", Permissions = GetTicketManagerPermissions() },
                new Role{ Name = "Collaborator", Description = "Colaborador que resolve tickets", Permissions = GetCollaboratorPermissions() },
                new Role{ Name = "SupportAnalyst", Description = "Analista de suporte", Permissions = GetSupportAnalystPermissions() },
                new Role{ Name = "AreaManager", Description = "Responsável pela área", Permissions = GetAreaManagerPermissions() },
                new Role{ Name = "SectorManager", Description = "Responsável pelo setor", Permissions = GetSectorManagerPermissions() }
            };

            _context.Roles.AddRange(roles);
            _context.SaveChanges();
        }
    }

    private List<Permission> GetMasterAdminPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "FullAccess" },
            new Permission { Name = "ManageUsers" },
            new Permission { Name = "ManageRoles" },
        };
    }

    private List<Permission> GetGeneralAdminPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ManageUsers" },
            new Permission { Name = "ManageDepartments" },
        };
    }

    private List<Permission> GetSectorAdminPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ManageSector" },
            new Permission { Name = "ViewTickets" },
        };
    }

    private List<Permission> GetDepartmentAdminPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ManageDepartment" },
            new Permission { Name = "ViewTickets" },
        };
    }

    private List<Permission> GetAreaAdminPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ManageArea" },
            new Permission { Name = "ViewTickets" },
        };
    }

    private List<Permission> GetTicketManagerPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ManageTickets" },
            new Permission { Name = "AssignTickets" },
        };
    }

    private List<Permission> GetCollaboratorPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ResolveTickets" },
            new Permission { Name = "ViewTickets" },
        };
    }

    private List<Permission> GetSupportAnalystPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ResolveTickets" },
            new Permission { Name = "ViewTicketHistory" },
        };
    }

    private List<Permission> GetAreaManagerPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ManageArea" },
            new Permission { Name = "ViewTickets" },
        };
    }

    private List<Permission> GetSectorManagerPermissions()
    {
        return new List<Permission>
        {
            new Permission { Name = "ManageSector" },
            new Permission { Name = "ViewTickets" },
        };
    }
}
