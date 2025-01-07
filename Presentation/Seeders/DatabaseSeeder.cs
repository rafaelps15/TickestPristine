using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Interfaces;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Seeders;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly TickestContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DatabaseSeeder(TickestContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Verifica se há dados nas tabelas e faz o seeding apenas se necessário
        if (!_context.Users.Any())
        {
            // Criando GUIDs
            var departmentId = Guid.NewGuid();
            var sectorId = Guid.NewGuid();
            var areaId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();

            var adminMasterId = Guid.NewGuid();
            string passwordAdmin = "@admin123";
            var (adminPasswordHash, adminSalt) = _passwordHasher.HashWithSalt(passwordAdmin);

            var adminRole = _context.Roles.FirstOrDefault(p => p.Name == TickestRoleDefaults.AdminMaster);

            // Criação de usuários
            var adminTeste = new User
            {
                Id = adminMasterId,
                Name = "AdminMaster",
                Email = "adminmaster@tickest.com",
                PasswordHash = adminPasswordHash,
                Salt = adminSalt,
                Role = adminRole, 
                IsActive = true
            };

            var testUserId = Guid.NewGuid();
            string passwordTeste = "@teste123";
            var (testPasswordHash, testSalt) = _passwordHasher.HashWithSalt(passwordTeste);

            var testeRole = _context.Roles.FirstOrDefault(p => p.Name == TickestRoleDefaults.AdminGeneral);
            var testUser = new User
            {
                Id = testUserId,
                Name = "Usuário Teste",
                Email = "testuser@tickest.com",
                PasswordHash = testPasswordHash,
                Salt = testSalt,
                Role = testeRole, 
                IsActive = true
            };
 
            //_context.Permissions.AddRange(permissions);
            _context.Users.Add(adminTeste);
            _context.Users.Add(testUser);

            // Criação de Departamentos
            var department = new Department
            {
                Id = departmentId,
                Name = "Tecnologia da Informação",
                Description = "Departamento responsável por gerenciar TI",
                SectorId = sectorId, 
                DepartmentManagerId = adminMasterId // Gestor do departamento
            };

            _context.Departments.Add(department);

            // Criação de Setores
            var sector = new Sector
            {
                Id = sectorId,
                Name = "Desenvolvimento",
                Description = "Setor de Desenvolvimento de Software",
                SectorManagerId = adminMasterId // Gestor do setor
            };

            _context.Sectors.Add(sector);

            // Criação de Áreas
            var area = new Area
            {
                Id = areaId,
                Name = "Desenvolvimento Backend",
                Description = "Foco no desenvolvimento de APIs",
                DepartmentId = departmentId, // Departamento ao qual a área pertence
            };

            _context.Areas.Add(area);

            // Criação de Especialidades
            var specialty = new Specialty
            {
                Id = specialtyId,
                Name = "ASP.NET Core",
                Description = "Desenvolvimento utilizando o framework ASP.NET Core"
            };

            _context.Specialties.Add(specialty);

            // Relacionamento entre usuário e área
            area.Users = new List<User> { testUser }; 

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
