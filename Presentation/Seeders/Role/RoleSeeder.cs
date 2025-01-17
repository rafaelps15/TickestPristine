//using System.Threading;
//using Tickest.Domain.Entities;
//using Tickest.Domain.Entities.Permissions;
//using Tickest.Domain.Interfaces.Repositories;
//using Tickest.Persistence.Data;
//using Tickest.Persistence.Repositories;

//namespace Tickest.Persistence.Seeders;

//public class RoleSeeder
//{
//    private readonly TickestContext _context;
//    private readonly IApplicationSettingRepository _applicationSettingRepository;
//    private readonly IRoleRepository _roleRepository;


//    public RoleSeeder(TickestContext context, IApplicationSettingRepository applicationSettingRepository)
//    {
//        _context = context;
//        _applicationSettingRepository = applicationSettingRepository;
//    }

//    public async Task SeedRoles(CancellationToken cancellationToken)
//    {
//        var seederFlag = await _applicationSettingRepository.GetSettingAsync("RolesSeeded");

//        if (seederFlag is null || seederFlag.Value != "True")
//        {
//            var rolesExist = await _roleRepository.AnyAsync(r => true, cancellationToken);

//            if (rolesExist == null)
//            {
//                var roles = new List<Role>
//                {
//                    new Role { Name = "AdminMaster", Description = "Controle total do Sistema", Permissions = GetAdminMasterPermissions() },
//                    new Role { Name = "AdminGeneral", Description = "Administração geral do sistema", Permissions = GetAdminGeneralPermissions() },
//                    new Role { Name = "SectorAdmin", Description = "Administração do setor", Permissions = GetSectorAdminPermissions() },
//                    new Role { Name = "DepartmentAdmin", Description = "Administração do departamento", Permissions = GetDepartmentAdminPermissions() },
//                    new Role { Name = "AreaAdmin", Description = "Administração da área", Permissions = GetAreaAdminPermissions() },
//                    new Role { Name = "TicketManager", Description = "Gestão de tickets da área, coordena a distribuição de tickets para especialistas", Permissions = GetTicketManagerPermissions() },
//                    new Role { Name = "Collaborator", Description = "Colaborador que abre o ticket", Permissions = GetCollaboratorPermissions() },
//                };

//                await _roleRepository.AddRangeAsync(roles);
//            }

//            // Atualizar ou configurar o valor para "RolesSeeded"
//            if (seederFlag is null)
//            {
//                await _applicationSettingRepository.SetSettingAsync(new ApplicationSetting
//                {
//                    Key = "RolesSeeded",
//                    Value = "True"
//                });
//            }
//            else
//            {
//                seederFlag.Value = "True";
//                await _applicationSettingRepository.SetSettingAsync(seederFlag);
//            }
//        }
//    }

//}