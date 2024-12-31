using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Entities.Departments;
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
        if (!_context.Departments.Any())
        {
            var departmentId = Guid.NewGuid();
            var sectorId = Guid.NewGuid();
            var areaId = Guid.NewGuid();
            var specialtyId = Guid.NewGuid();

            var adminMasterId = Guid.NewGuid();
            string passwordAdmin = "@admin123"; 
            var (adminPasswordHash, adminSalt) = _passwordHasher.HashWithSalt(passwordAdmin); 

            var adminMaster = new User
            {
                Id = adminMasterId,
                Name = "AdminMaster",
                Email = "adminmaster@tickest.com",
                PasswordHash = adminPasswordHash,
                Salt = adminSalt,
                Role = "AdminMaster",
                IsActive = true
            };

            var testUserId = Guid.NewGuid();
            string passwordTeste = "@teste123";
            var (testPasswordHash, testSalt) = _passwordHasher.HashWithSalt(passwordTeste);

            var testUser = new User
            {
                Id = testUserId,
                Name = "Usuário Teste",
                Email = "testuser@tickest.com",
                PasswordHash = testPasswordHash,
                Salt = testSalt,
                Role = "Analista",
                IsActive = true
            };

            _context.Users.Add(adminMaster);
            _context.Users.Add(testUser);

            _context.Departments.Add(new Department
            {
                Id = departmentId,
                Name = "Tecnologia da Informação",
                Description = "Departamento responsável por gerenciar TI"
            });

            _context.Sectors.Add(new Sector
            {
                Id = sectorId,
                Name = "Desenvolvimento",
                Description = "Setor de Desenvolvimento de Software",
                DepartmentId = departmentId
            });

            _context.Areas.Add(new Area
            {
                Id = areaId,
                Name = "Desenvolvimento Backend",
                Description = "Foco no desenvolvimento de APIs",
                SectorId = sectorId,
                ResponsibleUserId = testUserId, 
                SpecialtyId = specialtyId
            });

            _context.Specialties.Add(new Specialty
            {
                Id = specialtyId,
                Name = "ASP.NET Core",
                Description = "Desenvolvimento utilizando o framework ASP.NET Core"
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
