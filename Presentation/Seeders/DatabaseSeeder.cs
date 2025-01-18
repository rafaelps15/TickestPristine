using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Helpers;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        TickestContext context,
        IApplicationSettingRepository applicationSettingRepository,
        IPermissionRepository permissionRepository)
    {
        // Chama os seeders, verificando se já foram semeados utilizando o SeederHelper
        await PermissionSeeder.SeedPermissionsAsync(
            context,
            applicationSettingRepository,
            permissionRepository
        );

        // Você pode adicionar outros seeders aqui
        // await AnotherSeeder.SeedAsync(...);
    }
}


    // Seeding de outros dados, como configurações, usuários padrão, etc.


    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!_context.Users.Any())
        {
            // Gerando GUIDs para Departamentos, Setores, Áreas e Especialidades
            var departmentId = Guid.NewGuid();
            var sectorId = Guid.NewGuid();
            var areaId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();

            // Criando o salt e hash para a senha
            const string password = "@teste123";
            var salt = EncryptionHelper.CreateSaltKey(32);
            var passwordHash = EncryptionHelper.CreatePasswordHashWithSalt(password, salt);

            //Atribuição de roles 
            var adminMasterRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "AdminMaster");
            var adminGeneralRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "AdminGeneral");
            var sectorAdminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "SectorAdmin");
            var departmentAdminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "DepartmentAdmin");
            var areaAdminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "AreaAdmin");
            var ticketManagerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "TicketManager");
            var collaboratorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Collaborator");

            var roles = new[] {
                adminMasterRole,adminGeneralRole,sectorAdminRole,adminGeneralRole,departmentAdminRole,areaAdminRole,ticketManagerRole,collaboratorRole};

            if (!roles.Any(role => role == null))
            {
                _logger.LogError("Algumas roles não foram encontradas.");
                return;
            }

            // Criando usuários com nomes sequenciais diretamente
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste1",
                    Email = "usuarioteste1@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RoleId = adminMasterRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste2",
                    Email = "usuarioteste2@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RoleId = adminGeneralRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste3",
                    Email = "usuarioteste3@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RoleId = sectorAdminRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste4",
                    Email = "usuarioteste4@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RoleId = departmentAdminRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste5",
                    Email = "usuarioteste5@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RoleId = areaAdminRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste6",
                    Email = "usuarioteste6@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    RoleId = ticketManagerRole.Id,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste7",
                    Email = "usuarioteste7@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "Collaborator",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste8",
                    Email = "usuarioteste8@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "SupportAnalyst",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste9",
                    Email = "usuarioteste9@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "AreaManager",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "UsuarioTeste10",
                    Email = "usuarioteste10@tickest.com",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    //Role = "SectorManager",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                }
            };

            // Adicionando os usuários ao contexto
            _context.Users.AddRange(users);

            // Criação de Departamentos
            var department = new Department
            {
                Id = departmentId,
                Name = "Tecnologia da Informação",
                Description = "Departamento responsável por gerenciar TI",
                DepartmentManagerId = users.First(u => u.Name == "UsuarioTeste4").Id,
                CreatedAt = DateTime.Now
            };

            _context.Departments.Add(department);

            // Criação de Setores
            var sector = new Sector
            {
                Id = sectorId,
                Name = "Desenvolvimento",
                Description = "Setor de Desenvolvimento de Software",
                SectorManagerId = users.First(u => u.Name == "UsuarioTeste3").Id,
                CreatedAt = DateTime.Now
            };

            _context.Sectors.Add(sector);

            // Criação de Áreas
            var area = new Area
            {
                Id = areaId,
                Name = "Desenvolvimento Backend",
                Description = "Foco no desenvolvimento de APIs",
                DepartmentId = departmentId,
                AreaManagerId = users.First(u => u.Name == "UsuarioTeste5").Id,
                CreatedAt = DateTime.Now
            };

            _context.Areas.Add(area);

            // Criação de Especialidades
            var specialty1 = new Specialty
            {
                Id = specialtyId,
                Name = "ASP.NET Core",
                Description = "Desenvolvimento utilizando o framework ASP.NET Core para construir APIs robustas."
            };

            var specialty2 = new Specialty
            {
                Id = Guid.NewGuid(),
                Name = "Front End",
                Description = "Foco no desenvolvimento de interfaces de usuário com HTML, CSS e JavaScript."
            };

            // Adicionando as especialidades
            _context.Specialties.Add(specialty1);
            _context.Specialties.Add(specialty2);

            // Relacionando área com usuários
            area.Users = new List<User> { users.First(u => u.Name == "UsuarioTeste5") };

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}