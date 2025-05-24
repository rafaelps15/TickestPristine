using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities.Permissions;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public class RoleSeeder : IDatabaseSeeder
{
    public async Task SeedAsync(TickestContext context, CancellationToken cancellationToken = default)
    {
        if (!await context.Roles.AnyAsync(cancellationToken))
        {
            var roles = new List<Role>
            {
               new Role("AdminMaster", "Administrador do sistema com acesso total"),
               new Role("SectorAdmin", "Administrador de setor"),
               new Role("AdminGeneral", "Administrador geral do sistema"),
               new Role("DepartmentAdmin", "Administrador de departamento"),
               new Role("AreaAdmin", "Administrador de área"),
               new Role("TicketManager", "Gerente de tickets"),
               new Role("Collaborator", "Colaborador")
            };

            context.Roles.AddRange(roles);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
